// Decompiled with JetBrains decompiler
// Type: Game.AssetPipeline.AssetImportPipeline
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.AssetPipeline;
using Colossal.AssetPipeline.Collectors;
using Colossal.AssetPipeline.Diagnostic;
using Colossal.AssetPipeline.Importers;
using Colossal.AssetPipeline.Native;
using Colossal.AssetPipeline.PostProcessors;
using Colossal.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Colossal.IO.AssetDatabase.VirtualTexturing;
using Colossal.Json;
using Colossal.Logging;
using Colossal.PSI.Environment;
using Colossal.Reflection;
using Game.Prefabs;
using Game.Rendering;
using Game.Rendering.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

#nullable disable
namespace Game.AssetPipeline
{
  public static class AssetImportPipeline
  {
    private static readonly ILog log = LogManager.GetLogger("AssetPipeline");
    private const string kMainSettings = "settings.json";
    public static bool useParallelImport = true;
    public static ILocalAssetDatabase targetDatabase;
    private static readonly ProfilerMarker s_ImportPath = new ProfilerMarker("AssetImportPipeline.ImportPath");
    private static readonly ProfilerMarker s_ProfPostImport = new ProfilerMarker("AssetImportPipeline.PostImportMainThread");
    private static readonly ProfilerMarker s_ProfImportModels = new ProfilerMarker("AssetImportPipeline.ImportModels");
    private static readonly ProfilerMarker s_ProfImportTextures = new ProfilerMarker("AssetImportPipeline.ImportTextures");
    private static readonly ProfilerMarker s_ProfCreateGeomsSurfaces = new ProfilerMarker("AssetImportPipeline.CreateGeometriesAndSurfaces");
    private static readonly ProfilerMarker s_ProfImportAssetGroup = new ProfilerMarker("AssetImportPipeline.ImportAssetGroup");
    private static readonly ProfilerMarker s_ProfImportDidimo = new ProfilerMarker("AssetImportPipeline.ImportDidimo");
    private static readonly AssetImportPipeline.Progress s_Progress = new AssetImportPipeline.Progress();
    private static MainThreadDispatcher s_MainThreadDispatcher;
    public static Action<string, UnityEngine.Texture> OnDebugTexture;
    private static UnityEngine.Material s_BackgroundMaterial;
    private const float kBoundSize = 0.1f;
    private const float kHalfBoundSize = 0.05f;

    static AssetImportPipeline() => ImporterCache.GetSupportedExtensions();

    private static string GetNameWithoutGUID(string str) => str.Substring(0, str.LastIndexOf("_"));

    private static async System.Threading.Tasks.Task ExecuteMainThreadQueue(
      System.Threading.Tasks.Task importTask,
      Report report)
    {
      using (report.AddImportStep("Process main thread task queue"))
      {
        while (true)
        {
          if (importTask.IsCompleted)
            goto label_8;
label_2:
          if (!AssetImportPipeline.s_Progress.shouldCancel)
          {
            AssetImportPipeline.s_Progress.Update();
            if (AssetImportPipeline.s_MainThreadDispatcher.hasPendingTasks)
            {
              AssetImportPipeline.s_Progress.SetThreadDescription(string.Format("Executing {0} tasks", (object) AssetImportPipeline.s_MainThreadDispatcher.pendingTasksCount));
              AssetImportPipeline.s_MainThreadDispatcher.ProcessTasks();
            }
            await System.Threading.Tasks.Task.Yield();
            continue;
          }
          break;
label_8:
          if (AssetImportPipeline.s_MainThreadDispatcher.hasPendingTasks)
            goto label_2;
          else
            break;
        }
      }
      await importTask;
    }

    private static string MakeRelativePath(string path, string rootPath)
    {
      if (path.IndexOf(rootPath) == 0)
        return path.Substring(rootPath.Length + 1);
      throw new FormatException(path + " is not relative to " + rootPath);
    }

    public static void SetReportCallback(Func<string, string, float, bool> progressCallback)
    {
      AssetImportPipeline.s_Progress.Reset(progressCallback);
    }

    private static void AddSupportedThemes(string projectRootPath)
    {
      string path = projectRootPath + "/themes.json";
      if (!LongFile.Exists(path))
        return;
      Colossal.Json.Variant data = JSON.Load(LongFile.ReadAllText(path).Trim());
      if (data == null)
        return;
      AssetImportPipeline.ThemesConfig themesConfig = JSON.MakeInto<AssetImportPipeline.ThemesConfig>(data);
      if (themesConfig.themePrefixes == null)
        return;
      AssetUtils.AddSupportedThemes((IEnumerable<string>) themesConfig.themePrefixes);
      AssetImportPipeline.log.InfoFormat("Theme prefixes added: {0}", (object) string.Join(',', themesConfig.themePrefixes));
    }

    public static async System.Threading.Tasks.Task ImportPath(
      string projectRootPath,
      IEnumerable<string> relativePaths,
      ImportMode importMode,
      bool convertToVT,
      Func<string, string, float, bool> progressCallback = null,
      IPrefabFactory prefabFactory = null)
    {
      if (AssetImportPipeline.targetDatabase == null)
        throw new Exception("targetDatabase must be set");
      if (AssetImportPipeline.s_MainThreadDispatcher == null)
        AssetImportPipeline.s_MainThreadDispatcher = new MainThreadDispatcher();
      using (AssetImportPipeline.s_ImportPath.Auto())
      {
        Report report = new Report();
        int failures = 0;
        int total = 0;
        using (PerformanceCounter perf = PerformanceCounter.Start((Action<TimeSpan>) (t => AssetImportPipeline.log.Info((object) string.Format("Completed {0} asset groups import in {1:F3}s. Errors {2}. {3}", (object) total, (object) t.TotalSeconds, (object) failures, AssetImportPipeline.s_Progress.shouldCancel ? (object) "(Canceled)" : (object) "")))))
        {
          using (Report.ImportStep report1 = report.AddImportStep("Cache importers & post processors"))
          {
            ImporterCache.CacheSupportedExtensions(report1);
            PostProcessorCache.CachePostProcessors(report1);
          }
          AssetImportPipeline.SetReportCallback(progressCallback);
          AssetImportPipeline.AddSupportedThemes(projectRootPath);
          SourceAssetCollector assetCollector;
          using (report.AddImportStep("Collect source assets"))
          {
            AssetImportPipeline.s_Progress.Set("Importing assets", "Collecting files...", 0.0f);
            assetCollector = new SourceAssetCollector(projectRootPath, relativePaths);
          }
          ParallelOptions opts = new ParallelOptions()
          {
            MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
          };
          HashSet<SurfaceAsset> VTMaterials = new HashSet<SurfaceAsset>();
          int parallelCount = 0;
          await AssetImportPipeline.ExecuteMainThreadQueue((System.Threading.Tasks.Task) System.Threading.Tasks.Task.Run<ParallelLoopResult>((Func<ParallelLoopResult>) (() => Parallel.ForEach<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>>((IEnumerable<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>>) assetCollector, opts, (Action<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, ParallelLoopState, long>) ((asset, state, index) =>
          {
            string relativeRootPath = AssetImportPipeline.MakeRelativePath(asset.rootPath, projectRootPath);
            Report.Asset assetReport = report.AddAsset(asset.name);
            Interlocked.Increment(ref parallelCount);
            AssetImportPipeline.s_Progress.Set(string.Format("Importing {0} assets ({1}/{2})", (object) parallelCount, (object) (total + 1), (object) assetCollector.count), "Importing textures and meshes for " + asset.name, (float) total / (float) assetCollector.count);
            if (AssetImportPipeline.s_Progress.shouldCancel)
              state.Stop();
            Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory> postImportOperations;
            if (AssetImportPipeline.ImportAssetGroup(projectRootPath, relativeRootPath, asset, out List<List<Colossal.AssetPipeline.LOD>> _, out postImportOperations, report, assetReport))
              AssetImportPipeline.s_MainThreadDispatcher.Dispatch((System.Action) (() =>
              {
                Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory> action = postImportOperations;
                if (action == null)
                  return;
                action(relativeRootPath, importMode, report, VTMaterials, prefabFactory);
              }));
            else
              Interlocked.Increment(ref failures);
            Interlocked.Increment(ref total);
            Interlocked.Decrement(ref parallelCount);
            AssetImportPipeline.s_Progress.Set(string.Format("Importing {0} assets ({1}/{2})", (object) parallelCount, (object) (total + 1), (object) assetCollector.count), "Completed textures and meshes for " + asset.name, (float) total / (float) assetCollector.count);
          })))), report);
          if (convertToVT)
          {
            using (Report.ImportStep report2 = report.AddImportStep("Convert materials to VT"))
              AssetImportPipeline.ProcessSurfacesForVT((IEnumerable<SurfaceAsset>) VTMaterials, (IEnumerable<SurfaceAsset>) null, (importMode & ImportMode.Forced) == ImportMode.Forced, report2);
          }
          AssetImportPipeline.s_Progress.Set("Completed", "", 1f);
          report.totalTime = perf.result;
        }
        report.Log(AssetImportPipeline.log);
        AssetImportPipeline.s_MainThreadDispatcher = (MainThreadDispatcher) null;
      }
    }

    private static Colossal.AssetPipeline.Settings ImportSettings(
      SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset> assetGroup,
      (Report.ImportStep step, Report.Asset asset) report)
    {
      using (Report.ImportStep report1 = report.step.AddImportStep("Settings import"))
      {
        bool flag = false;
        Colossal.AssetPipeline.Settings settings = Colossal.AssetPipeline.Settings.GetDefault(assetGroup.name);
        foreach (SourceAssetCollector.Asset asset in assetGroup)
        {
          if (ImporterCache.GetImporter(Path.GetExtension(asset.path)) is SettingsImporter importer && asset.name == "settings.json")
          {
            flag = true;
            Report.FileReport report2 = report.asset.AddFile(asset);
            importer.Import(asset.path, ref settings, report2);
          }
        }
        if (!flag)
        {
          SettingsImporter.Expand(ref settings, (ReportBase) report1);
          report1.AddMessage("Using default settings: " + settings.ToJSONString<Colossal.AssetPipeline.Settings>());
        }
        return settings;
      }
    }

    private static string ResolveRelativePath(string projectRootPath, string target, string to)
    {
      return target.StartsWith('/') ? Path.GetFullPath(Path.Combine(projectRootPath, target.Substring(1))) : Path.GetFullPath(Path.Combine(to, target));
    }

    private static SourceAssetCollector.AssetGroup<IAsset> CreateAssetGroupFromSettings(
      string projectRootPath,
      Colossal.AssetPipeline.Settings settings,
      SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset> assetGroup,
      Report.Asset assetReport)
    {
      HashSet<IAsset> files = new HashSet<IAsset>(assetGroup.count);
      foreach (SourceAssetCollector.Asset sourceAsset in assetGroup)
      {
        if (settings.ignoreSuffixes == null || !Path.GetFileNameWithoutExtension(sourceAsset.name).EndsWithAny(settings.ignoreSuffixes))
        {
          IAsset asset = IAsset.Create(settings, Path.GetFileNameWithoutExtension(sourceAsset.name), sourceAsset);
          if (asset != null)
            files.Add(asset);
        }
      }
      foreach (KeyValuePair<string, string> usedShaderAsset in (IEnumerable<KeyValuePair<string, string>>) settings.UsedShaderAssets(assetGroup, assetReport))
      {
        string path = AssetImportPipeline.ResolveRelativePath(projectRootPath, usedShaderAsset.Value, assetGroup.rootPath);
        if (!LongFile.Exists(path))
        {
          string fileName = Path.GetFileName(path);
          path = EnvPath.kContentPath + "/Game/.ModdingToolchain/shared_assets_fallback/" + fileName;
          AssetImportPipeline.log.InfoFormat("Using fallback {0}", (object) fileName);
        }
        if (LongFile.Exists(path))
        {
          SourceAssetCollector.Asset sourceAsset = new SourceAssetCollector.Asset(path, projectRootPath);
          IAsset asset = IAsset.Create(settings, Path.GetFileNameWithoutExtension(usedShaderAsset.Key), sourceAsset);
          if (asset != null)
            files.Add(asset);
        }
      }
      return new SourceAssetCollector.AssetGroup<IAsset>(assetGroup.rootPath, files);
    }

    private static void ImportModels(
      Colossal.AssetPipeline.Settings settings,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      (Report.ImportStep step, Report.Asset asset) report)
    {
      using (AssetImportPipeline.s_ProfImportModels.Auto())
      {
        using (Report.ImportStep modelsReport = report.step.AddImportStep("Import Models"))
        {
          ParallelOptions parallelOptions = new ParallelOptions()
          {
            MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
          };
          int failures = 0;
          int total = 0;
          using (PerformanceCounter.Start((Action<TimeSpan>) (t =>
          {
            if (total == 0)
              AssetImportPipeline.log.Info((object) string.Format("No models processed. All models in this group were already loaded or none were found. {0:F3}", (object) t.TotalSeconds));
            else
              AssetImportPipeline.log.Info((object) string.Format("Completed {0} models import in {1:F3}s. Errors {2}.", (object) total, (object) t.TotalSeconds, (object) failures));
          })))
            Parallel.ForEach<ModelAsset>(assetGroup.FilterBy<ModelAsset>(), parallelOptions, (Action<ModelAsset, ParallelLoopState, long>) ((asset, state, index) =>
            {
              if (asset.instance != null)
                return;
              Report.FileReport report1 = report.asset.AddFile((IAsset) asset);
              using (AssetImportPipeline.s_Progress.ScopedThreadDescription("Importing " + asset.fileName))
              {
                if (AssetImportPipeline.s_Progress.shouldCancel)
                  state.Stop();
                try
                {
                  ISettings importSettings = settings.GetImportSettings(asset.fileName, (IAssetImporter) asset.importer, (ReportBase) report1);
                  using (modelsReport.AddImportStep("Asset import"))
                  {
                    if (!asset.importer.Import<ModelImporter.ModelList>(importSettings, asset.path, report1, out asset.instance))
                      return;
                    asset.instance.sourceAsset = (IAsset) asset;
                    Interlocked.Increment(ref total);
                    if (!asset.instance.isValid)
                      return;
                    foreach (IModelPostProcessor modelPostProcessor in (IEnumerable<IModelPostProcessor>) PostProcessorCache.GetModelPostProcessors())
                    {
                      ISettings settings1;
                      if (settings.GetPostProcessSettings(asset.fileName, (ISettingable) modelPostProcessor, (ReportBase) report1, out settings1))
                      {
                        Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings1, settings);
                        if (modelPostProcessor.ShouldExecute(context, asset))
                        {
                          using (modelsReport.AddImportStep("Execute " + PPUtils.GetPostProcessorString(modelPostProcessor.GetType()) + " Post Processors"))
                            modelPostProcessor.Execute(context, asset, report1);
                        }
                      }
                    }
                  }
                }
                catch (Exception ex)
                {
                  Interlocked.Increment(ref failures);
                  AssetImportPipeline.log.Error(ex, (object) ("Error importing " + asset.name + ". Skipped..."));
                  report1.AddError(string.Format("Error: {0}", (object) ex));
                }
              }
            }));
        }
      }
    }

    private static void ImportDidimo(
      Colossal.AssetPipeline.Settings settings,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      (Report.ImportStep step, Report.Asset asset) report)
    {
      using (AssetImportPipeline.s_ProfImportDidimo.Auto())
      {
        using (Report.ImportStep importReport = report.step.AddImportStep("Import didimo"))
        {
          int failures = 0;
          int total = 0;
          ParallelOptions parallelOptions = new ParallelOptions()
          {
            MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
          };
          using (PerformanceCounter.Start((Action<TimeSpan>) (t => AssetImportPipeline.log.Info((object) string.Format("Completed {0} animations import in {1:F3}s. Errors {2}.", (object) total, (object) t.TotalSeconds, (object) failures)))))
            Parallel.ForEach<Colossal.AssetPipeline.AnimationAsset>(assetGroup.FilterBy<Colossal.AssetPipeline.AnimationAsset>(), parallelOptions, (Action<Colossal.AssetPipeline.AnimationAsset, ParallelLoopState, long>) ((asset, state, index) =>
            {
              Report.FileReport report1 = report.asset.AddFile((IAsset) asset);
              using (AssetImportPipeline.s_Progress.ScopedThreadDescription("Importing " + asset.fileName))
              {
                if (AssetImportPipeline.s_Progress.shouldCancel)
                  state.Stop();
                try
                {
                  ISettings importSettings = settings.GetImportSettings(asset.fileName, (IAssetImporter) asset.importer, (ReportBase) report1);
                  using (importReport.AddImportStep("Animations import"))
                  {
                    if (!asset.importer.Import<DidimoImporter.AnimationData>(importSettings, asset.path, report1, out asset.instance))
                      return;
                    asset.instance.sourceAsset = (IAsset) asset;
                    Interlocked.Increment(ref total);
                    int num = asset.instance.isValid ? 1 : 0;
                  }
                }
                catch (Exception ex)
                {
                  Interlocked.Increment(ref failures);
                  AssetImportPipeline.log.Error(ex, (object) ("Error importing " + asset.name + ". Skipped..."));
                  report1.AddError(string.Format("Error: {0}", (object) ex));
                }
              }
            }));
          failures = 0;
          total = 0;
          using (PerformanceCounter.Start((Action<TimeSpan>) (t => AssetImportPipeline.log.Info((object) string.Format("Completed {0} didimo asset import in {1:F3}s. Errors {2}.", (object) total, (object) t.TotalSeconds, (object) failures)))))
            Parallel.ForEach<DidimoAsset>(assetGroup.FilterBy<DidimoAsset>(), parallelOptions, (Action<DidimoAsset, ParallelLoopState, long>) ((asset, state, index) =>
            {
              Report.FileReport report2 = report.asset.AddFile((IAsset) asset);
              using (AssetImportPipeline.s_Progress.ScopedThreadDescription("Importing " + asset.fileName))
              {
                if (AssetImportPipeline.s_Progress.shouldCancel)
                  state.Stop();
                try
                {
                  ISettings importSettings = settings.GetImportSettings(asset.fileName, (IAssetImporter) asset.importer, (ReportBase) report2);
                  using (importReport.AddImportStep("Asset import"))
                  {
                    if (!asset.importer.Import<DidimoImporter.DidimoData>(importSettings, asset.path, report2, out asset.instance))
                      return;
                    asset.instance.sourceAsset = (IAsset) asset;
                    Interlocked.Increment(ref total);
                    int num = asset.instance.isValid ? 1 : 0;
                  }
                }
                catch (Exception ex)
                {
                  Interlocked.Increment(ref failures);
                  AssetImportPipeline.log.Error(ex, (object) ("Error importing " + asset.name + ". Skipped..."));
                  report2.AddError(string.Format("Error: {0}", (object) ex));
                }
              }
            }));
        }
      }
    }

    private static void CreateDidimoAssets(
      Colossal.AssetPipeline.Settings settings,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      out Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory> postImportOperations,
      (Report parent, Report.ImportStep step, Report.Asset asset) report1)
    {
      postImportOperations = (Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory>) null;
      using (Report.ImportStep geometryReport = report1.parent.AddImportStep("Create Geometry and Surfaces"))
      {
        ParallelOptions opts = new ParallelOptions()
        {
          MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
        };
        List<(string, IReadOnlyList<(Colossal.Animations.Animation, Colossal.Animations.BoneHierarchy, string, int, Colossal.Hash128)>, int, int)> characterStyles = new List<(string, IReadOnlyList<(Colossal.Animations.Animation, Colossal.Animations.BoneHierarchy, string, int, Colossal.Hash128)>, int, int)>();
        List<(List<Colossal.AssetPipeline.LOD>, string)> allRenderGroups = new List<(List<Colossal.AssetPipeline.LOD>, string)>();
        List<(string, IReadOnlyList<(int, CharacterGroup.Meta, IReadOnlyList<int>)>)> groups = new List<(string, IReadOnlyList<(int, CharacterGroup.Meta, IReadOnlyList<int>)>)>();
        string[] defaultTextures = new string[8]
        {
          "Identity512_MaskMap.png",
          "Identity1024_MaskMap.png",
          "Identity2048_MaskMap.png",
          "Identity4096_MaskMap.png",
          "White512_ControlMask.png",
          "White1024_ControlMask.png",
          "White2048_ControlMask.png",
          "White4096_ControlMask.png"
        };
        AssetImportPipeline.ImportTextures(settings, relativeRootPath, assetGroup, (Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => ((IEnumerable<string>) defaultTextures).Contains<string>(x.fileName)), (report1.step, report1.asset));
        foreach (DidimoAsset didimoAsset in assetGroup.FilterBy<DidimoAsset>())
        {
          DidimoAsset asset = didimoAsset;
          DidimoImporter.DidimoData didimo = asset.instance;
          if (didimo != null)
          {
            int parallelCount = 0;
            (List<Colossal.AssetPipeline.LOD>, string)[] renderGroups = new (List<Colossal.AssetPipeline.LOD>, string)[didimo.renderGroups.Count];
            Parallel.ForEach<DidimoImporter.DidimoData.RenderGroup>((IEnumerable<DidimoImporter.DidimoData.RenderGroup>) didimo.renderGroups, opts, (Action<DidimoImporter.DidimoData.RenderGroup, ParallelLoopState, long>) ((renderGroup, rgState, rgIndex) =>
            {
              try
              {
                ModelImporter.Model[] models = new ModelImporter.Model[renderGroup.renderObjects.Length];
                Surface[] surfaces = new Surface[renderGroup.renderObjects.Length];
                Parallel.ForEach<DidimoImporter.DidimoData.RenderObject>((IEnumerable<DidimoImporter.DidimoData.RenderObject>) renderGroup.renderObjects, opts, (Action<DidimoImporter.DidimoData.RenderObject, ParallelLoopState, long>) ((renderObject, roState, roIndex) =>
                {
                  AssetImportPipeline.s_Progress.Set(string.Format("Importing {0} render objects", (object) parallelCount), "Importing textures and meshes for " + asset.name);
                  Interlocked.Increment(ref parallelCount);
                  DidimoImporter.DidimoData.Mesh mesh = renderObject.mesh;
                  Matrix4x4 identity = Matrix4x4.identity;
                  NativeArray<int> indices = new NativeArray<int>(mesh.indices, Allocator.Persistent);
                  ModelImporter.Model.VertexData[] array = new ModelImporter.Model.VertexData[4]
                  {
                    new ModelImporter.Model.VertexData(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.float3>(mesh.vertices)),
                    new ModelImporter.Model.VertexData(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.float3>(mesh.normals)),
                    new ModelImporter.Model.VertexData(VertexAttribute.Tangent, VertexAttributeFormat.Float32, 4, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.float4>(mesh.tangents)),
                    new ModelImporter.Model.VertexData(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.float2>(mesh.uv))
                  };
                  if (mesh.boneWeights.Length != 0)
                  {
                    int length = array.Length;
                    System.Array.Resize<ModelImporter.Model.VertexData>(ref array, length + 2);
                    array[length] = new ModelImporter.Model.VertexData(VertexAttribute.BlendIndices, VertexAttributeFormat.UInt32, 4, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.IndexWeight4>(mesh.boneWeights, 0, 4, 4));
                    array[length + 1] = new ModelImporter.Model.VertexData(VertexAttribute.BlendWeight, VertexAttributeFormat.Float32, 4, AssetImportPipeline.FromManagedArray<DidimoImporter.DidimoData.IndexWeight4>(mesh.boneWeights, 4, 4, 4));
                  }
                  string withoutExtension = Path.GetFileNameWithoutExtension(mesh.name);
                  if (didimo.props.Any<DidimoImporter.DidimoData.Prop>((Func<DidimoImporter.DidimoData.Prop, bool>) (x => (long) x.renderGroupIndex == rgIndex)))
                    withoutExtension += "_Prop";
                  string name3 = AssetImportPipeline.AdjustNamingConvention(withoutExtension);
                  SubMeshDescriptor subMesh = ModelImporter.Model.CreateSubMesh(name3, mesh.indexCount, mesh.vertexCount, indices, array);
                  ModelAsset model3 = new ModelAsset(name3, asset.collectedAsset, (ModelImporter) asset.importer, false);
                  report1.parent.AddAsset(name3);
                  ModelImporter.Model model4 = new ModelImporter.Model(name3, identity, mesh.vertexCount, indices, array, new SubMeshDescriptor[1]
                  {
                    subMesh
                  }, -1, (ModelImporter.Model.BoneInfo[]) null);
                  model4.sourceAsset = (IAsset) model3;
                  DidimoImporter.DidimoData.Material material = renderObject.material;
                  string name4 = AssetImportPipeline.AdjustNamingConvention(Path.GetFileNameWithoutExtension(material.name));
                  if (models.Length > 1)
                    name4 += string.Format("#{0}", (object) roIndex);
                  Surface surface = new Surface(name4, Constants.Material.Shader.GetCharacterShader(material.name, material.shader));
                  Report.AssetData assetData = report1.parent.AddAssetData(surface.name, typeof (Surface));
                  material.textures.Add(new DidimoImporter.DidimoData.Material.Texture()
                  {
                    index = -1,
                    key = "_ControlMask"
                  });
                  int num4 = 0;
                  foreach (DidimoImporter.DidimoData.Material.Texture texture in material.textures)
                  {
                    DidimoImporter.DidimoData.Texture textureData;
                    if (texture.index == -1)
                      textureData = new DidimoImporter.DidimoData.Texture()
                      {
                        path = new string[1]
                        {
                          name4 + "_ControlMask.png"
                        }
                      };
                    else if (texture.key == "_MaskMap" && didimo.textures[texture.index].path[0] == "Green.png" || didimo.textures[texture.index].path[0] == "Male_Beard01_BaseColor.png")
                    {
                      report1.asset.AddMessage(didimo.textures[texture.index].path[0] + " was removed by import for " + name4);
                      textureData = new DidimoImporter.DidimoData.Texture()
                      {
                        path = new string[1]{ string.Empty }
                      };
                    }
                    else
                      textureData = didimo.textures[texture.index];
                    AssetImportPipeline.ImportTextures(settings, relativeRootPath, assetGroup, (Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => ((IEnumerable<string>) textureData.path).Contains<string>(x.fileName)), (report1.step, report1.asset));
                    if (textureData.path.Length == 1)
                    {
                      TextureImporter.Texture instance4 = assetGroup.Find<Colossal.AssetPipeline.TextureAsset>((Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => x.fileName == textureData.path[0]))?.instance;
                      if (instance4 != null)
                      {
                        num4 = instance4.width;
                        surface.AddProperty(texture.key, (TextureImporter.ITexture) instance4);
                      }
                      else if (texture.key == "_MaskMap" && num4 != 0)
                      {
                        int resolution = num4;
                        TextureImporter.Texture instance5 = assetGroup.Find<Colossal.AssetPipeline.TextureAsset>((Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => x.fileName == string.Format("Identity{0}{1}.png", (object) resolution, (object) "_MaskMap")))?.instance;
                        if (instance5 != null)
                          surface.AddProperty(texture.key, (TextureImporter.ITexture) instance5);
                      }
                      else if (texture.key == "_ControlMask" && num4 != 0)
                      {
                        int resolution = num4;
                        TextureImporter.Texture instance6 = assetGroup.Find<Colossal.AssetPipeline.TextureAsset>((Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => x.fileName == string.Format("White{0}{1}.png", (object) resolution, (object) "_ControlMask")))?.instance;
                        if (instance6 != null)
                          surface.AddProperty(texture.key, (TextureImporter.ITexture) instance6);
                      }
                    }
                    else
                    {
                      int num5 = 0;
                      int num6 = 0;
                      for (int index = 0; index < textureData.path.Length; ++index)
                      {
                        int i1 = index;
                        int width = assetGroup.Find<Colossal.AssetPipeline.TextureAsset>((Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => x.fileName == textureData.path[i1])).instance.width;
                        if (width > num6)
                        {
                          num5 = index;
                          num6 = width;
                        }
                      }
                      TextureImporter.TextureArray textureArray = new TextureImporter.TextureArray();
                      for (int index = num5; index < textureData.path.Length; ++index)
                      {
                        int i1 = index;
                        if (!textureArray.AddSlice(assetGroup.Find<Colossal.AssetPipeline.TextureAsset>((Func<Colossal.AssetPipeline.TextureAsset, bool>) (x => x.fileName == textureData.path[i1])).instance))
                          AssetImportPipeline.log.WarnFormat("Texture {0} does not match the texture array resolution {1}x{2}. Skipped!", (object) textureData.path[i1], (object) textureArray.width, (object) textureArray.height);
                      }
                      surface.AddProperty(texture.key, (TextureImporter.ITexture) textureArray);
                      if (num5 > 0)
                        surface.AddProperty(texture.key + "_IndexOffset", (float) -num5);
                    }
                  }
                  Report.FileReport fileReport = report1.parent.GetFileReport((IAsset) model3);
                  foreach (IModelSurfacePostProcessor surfacePostProcessor in (IEnumerable<IModelSurfacePostProcessor>) PostProcessorCache.GetModelSurfacePostProcessors())
                  {
                    ISettings settings2;
                    if (settings.GetPostProcessSettings(model4.name, (ISettingable) surfacePostProcessor, (ReportBase) fileReport, out settings2))
                    {
                      Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings2, settings);
                      if (surfacePostProcessor.ShouldExecute(context, model3, 0, surface))
                      {
                        using (geometryReport.AddImportStep("Execute " + PPUtils.GetPostProcessorString(surfacePostProcessor.GetType()) + " Post Processors"))
                          surfacePostProcessor.Execute(context, model3, 0, surface, (report1.parent, report1.asset, fileReport, assetData));
                      }
                    }
                  }
                  if (renderObject.shapeBuffer.stride != 0 && renderObject.shapeBuffer.elements != null)
                  {
                    if (renderObject.shapeBuffer.stride != mesh.vertexCount)
                      throw new ModelImportException(string.Format("Error importing {0}: mesh {1} vertex count ({2}) does not match shape stride ({3})", (object) asset.name, (object) mesh.name, (object) mesh.vertexCount, (object) renderObject.shapeBuffer.stride));
                    if (renderObject.shapeBuffer.elements.Length % renderObject.shapeBuffer.stride != 0)
                      throw new ModelImportException(string.Format("Error importing {0}: mesh {1} shape buffer size ({2}) is not multiple of stride ({3})", (object) asset.name, (object) mesh.name, (object) renderObject.shapeBuffer.elements.Length, (object) renderObject.shapeBuffer.stride));
                    NativeArray<DidimoImporter.DidimoData.ShapeBuffer.Element> nativeArray = new NativeArray<DidimoImporter.DidimoData.ShapeBuffer.Element>(renderObject.shapeBuffer.elements, Allocator.Persistent);
                    int count = nativeArray.Length / renderObject.shapeBuffer.stride;
                    model4.SetShapeData(nativeArray.Reinterpret<byte>(24), count);
                  }
                  surfaces[roIndex] = surface;
                  models[roIndex] = model4;
                  Interlocked.Decrement(ref parallelCount);
                }));
                int firstShapeCount = ((IEnumerable<ModelImporter.Model>) models).First<ModelImporter.Model>().shapeCount;
                if (((IEnumerable<ModelImporter.Model>) models).Any<ModelImporter.Model>((Func<ModelImporter.Model, bool>) (m => m.shapeCount != firstShapeCount)))
                  throw new ModelImportException(string.Format("Error importing {0}: not all meshes use the same shape count (first shape count {1})", (object) asset.name, (object) firstShapeCount));
                List<Colossal.AssetPipeline.LOD> asset1 = new List<Colossal.AssetPipeline.LOD>();
                Geometry geometry = new Geometry(models);
                asset1.Add(new Colossal.AssetPipeline.LOD(geometry, surfaces, 0));
                Report.AssetData assetData1 = report1.parent.AddAssetData(geometry.name, typeof (Geometry));
                assetData1.AddFiles(((IEnumerable<ModelImporter.Model>) models).Select<ModelImporter.Model, IAsset>((Func<ModelImporter.Model, IAsset>) (t => t.sourceAsset)));
                Report.AssetData[] assetDataArray = new Report.AssetData[surfaces.Length];
                int index1 = 0;
                foreach (Surface surface in surfaces)
                {
                  assetDataArray[index1] = report1.parent.AddAssetData(surface.name, typeof (Surface));
                  assetDataArray[index1++].AddFiles(surface.textures.Values.Select<TextureImporter.ITexture, IAsset>((Func<TextureImporter.ITexture, IAsset>) (t => t.sourceAsset)));
                }
                foreach (IGeometryPostProcessor geometryPostProcessor in (IEnumerable<IGeometryPostProcessor>) PostProcessorCache.GetGeometryPostProcessors())
                {
                  try
                  {
                    ISettings settings3;
                    if (settings.GetPostProcessSettings(geometry.name, (ISettingable) geometryPostProcessor, (ReportBase) report1.asset, out settings3))
                    {
                      Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings3, settings);
                      if (geometryPostProcessor.ShouldExecute(context, asset1))
                      {
                        using (geometryReport.AddImportStep("Execute " + PPUtils.GetPostProcessorString(geometryPostProcessor.GetType()) + " Post Processors"))
                          geometryPostProcessor.Execute(context, asset1, (report1.parent, report1.asset, assetData1, assetDataArray));
                      }
                    }
                  }
                  catch (Exception ex)
                  {
                    AssetImportPipeline.log.Error(ex, (object) ("Exception occured with " + geometry.name));
                    throw;
                  }
                }
                renderGroups[rgIndex] = (asset1, renderGroup.bodyParts);
              }
              catch (Exception ex)
              {
                AssetImportPipeline.log.Error(ex, (object) ("Exception occured with " + asset.name));
              }
            }));
            int count1 = allRenderGroups.Count;
            allRenderGroups.AddRange((IEnumerable<(List<Colossal.AssetPipeline.LOD>, string)>) renderGroups);
            HashSet<int> intSet = new HashSet<int>();
            System.Collections.Generic.Dictionary<string, (int, string)> dictionary = new System.Collections.Generic.Dictionary<string, (int, string)>();
            for (int index = 0; index < didimo.props.Count; ++index)
            {
              DidimoImporter.DidimoData.Prop prop = didimo.props[index];
              intSet.Add(prop.renderGroupIndex);
              dictionary.Add(prop.name, (index, (string) null));
            }
            foreach (DidimoImporter.DidimoData.AnimationGroup animationGroup1 in didimo.animationGroups)
            {
              DidimoImporter.DidimoData.AnimationGroup animationGroup = animationGroup1;
              Colossal.Animations.BoneHierarchy boneHierarchy1 = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.BoneHierarchy, Colossal.Animations.BoneHierarchy>(animationGroup.boneHierarchy);
              List<(Colossal.Animations.Animation, Colossal.Animations.BoneHierarchy, string, int, Colossal.Hash128)> valueTupleList1 = new List<(Colossal.Animations.Animation, Colossal.Animations.BoneHierarchy, string, int, Colossal.Hash128)>(animationGroup.paths.Count + 1);
              Colossal.Animations.Animation animation1 = new Colossal.Animations.Animation()
              {
                name = animationGroup.styleName + "_RestPose",
                type = Colossal.Animations.AnimationType.RestPose,
                layer = Colossal.Animations.AnimationLayer.All,
                shapeIndices = new int[animationGroup.shapeCount],
                boneIndices = new int[animationGroup.boneCount],
                frameCount = 1
              };
              for (int index = 0; index < animationGroup.shapeCount; ++index)
                animation1.shapeIndices[index] = index;
              for (int index = 0; index < animationGroup.boneCount; ++index)
                animation1.boneIndices[index] = index;
              animation1.SetElements(Span<Colossal.Animations.Animation.ElementRaw>.op_Implicit(MemoryMarshal.Cast<DidimoImporter.DidimoData.Animation.Element, Colossal.Animations.Animation.ElementRaw>(Span<DidimoImporter.DidimoData.Animation.Element>.op_Implicit(animationGroup.restPose.elements))));
              valueTupleList1.Add((animation1, boneHierarchy1, "RestPose", -1, HashUtils.GetHash(animation1, relativeRootPath)));
              List<(string, int)> valueTupleList2 = new List<(string, int)>();
label_26:
              for (int i = 0; i < animationGroup.paths.Count; ++i)
              {
                DidimoImporter.DidimoData.Animation animation2 = assetGroup.FilterBy<Colossal.AssetPipeline.AnimationAsset>((Func<Colossal.AssetPipeline.AnimationAsset, bool>) (a => animationGroup.paths[i].Contains(a.fileName))).Select<Colossal.AssetPipeline.AnimationAsset, DidimoImporter.AnimationData>((Func<Colossal.AssetPipeline.AnimationAsset, DidimoImporter.AnimationData>) (a => a.instance)).First<DidimoImporter.AnimationData>().m_Animation;
                if (!string.IsNullOrEmpty(animation2.targetName))
                {
                  (int, string) valueTuple1;
                  if (!dictionary.TryGetValue(animation2.targetName, out valueTuple1))
                    throw new ModelImportException("Error importing " + asset.name + ": animation target model not found (" + animation2.targetName + ")");
                  foreach ((string, int) valueTuple2 in valueTupleList2)
                  {
                    if (animation2.name.StartsWith(valueTuple2.Item1))
                      goto label_26;
                  }
                  valueTupleList2.Add((animation2.name, valueTuple1.Item1));
                }
              }
              for (int i = 0; i < animationGroup.paths.Count; ++i)
              {
                DidimoImporter.AnimationData animationData = assetGroup.FilterBy<Colossal.AssetPipeline.AnimationAsset>((Func<Colossal.AssetPipeline.AnimationAsset, bool>) (a => animationGroup.paths[i].Contains(a.fileName))).Select<Colossal.AssetPipeline.AnimationAsset, DidimoImporter.AnimationData>((Func<Colossal.AssetPipeline.AnimationAsset, DidimoImporter.AnimationData>) (a => a.instance)).First<DidimoImporter.AnimationData>();
                DidimoImporter.DidimoData.Animation animation3 = animationData.m_Animation;
                Colossal.Animations.Animation animation4 = new Colossal.Animations.Animation();
                animation4.type = (Colossal.Animations.AnimationType) animation3.animationType;
                animation4.layer = (Colossal.Animations.AnimationLayer) animation3.layerIndex;
                animation4.shapeIndices = animation3.shapeIndices;
                animation4.boneIndices = animation3.boneIndices;
                animation4.frameCount = animation3.frameCount;
                animation4.frameRate = animation3.fps;
                Colossal.Animations.Animation animation5 = animation4;
                animation5.SetElements(Span<Colossal.Animations.Animation.ElementRaw>.op_Implicit(MemoryMarshal.Cast<DidimoImporter.DidimoData.Animation.Element, Colossal.Animations.Animation.ElementRaw>(Span<DidimoImporter.DidimoData.Animation.Element>.op_Implicit(animation3.elements))));
                Colossal.Animations.BoneHierarchy boneHierarchy2 = boneHierarchy1;
                int num = -1;
                if (!string.IsNullOrEmpty(animation3.targetName))
                {
                  (int, string) valueTuple;
                  if (!dictionary.TryGetValue(animation3.targetName, out valueTuple))
                    throw new ModelImportException("Error importing " + asset.name + ": animation target model not found (" + animation3.targetName + ")");
                  DidimoImporter.DidimoData.Prop prop = didimo.props[valueTuple.Item1];
                  boneHierarchy2 = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.BoneHierarchy, Colossal.Animations.BoneHierarchy>(prop.boneHierarchy);
                  num = prop.renderGroupIndex + count1;
                  animation5.name = animationGroup.styleName + "_" + animation3.name + "#" + Path.GetFileNameWithoutExtension(animation3.targetName);
                  if (valueTuple.Item2 != animationGroup.styleName)
                  {
                    valueTuple.Item2 = animationGroup.styleName;
                    dictionary[animation3.targetName] = valueTuple;
                    animation4 = new Colossal.Animations.Animation();
                    animation4.name = "RestPose#" + Path.GetFileNameWithoutExtension(animation3.targetName);
                    animation4.type = Colossal.Animations.AnimationType.RestPose;
                    animation4.layer = Colossal.Animations.AnimationLayer.PropLayer;
                    animation4.shapeIndices = new int[1];
                    animation4.boneIndices = new int[prop.boneHierarchy.hierarchyParentIndices.Length];
                    animation4.frameCount = 1;
                    Colossal.Animations.Animation animation6 = animation4;
                    for (int index = 0; index < animation6.boneIndices.Length; ++index)
                      animation6.boneIndices[index] = index;
                    animation6.SetElements(Span<Colossal.Animations.Animation.ElementRaw>.op_Implicit(MemoryMarshal.Cast<DidimoImporter.DidimoData.Animation.Element, Colossal.Animations.Animation.ElementRaw>(Span<DidimoImporter.DidimoData.Animation.Element>.op_Implicit(prop.restPose.elements))));
                    valueTupleList1.Add((animation6, boneHierarchy2, "RestPose", num, HashUtils.GetHash(animation6, relativeRootPath)));
                  }
                }
                else
                {
                  foreach ((string, int) valueTuple in valueTupleList2)
                  {
                    if (animation3.name.StartsWith(valueTuple.Item1))
                    {
                      num = didimo.props[valueTuple.Item2].renderGroupIndex;
                      num += count1;
                      break;
                    }
                  }
                  animation5.name = animationGroup.styleName + "_" + animation3.name;
                }
                valueTupleList1.Add((animation5, boneHierarchy2, animation3.name, num, HashUtils.GetHash(animation5, animationData.sourceAsset.path)));
              }
              characterStyles.Add((animationGroup.styleName, (IReadOnlyList<(Colossal.Animations.Animation, Colossal.Animations.BoneHierarchy, string, int, Colossal.Hash128)>) valueTupleList1, animationGroup.boneCount, animationGroup.shapeCount));
            }
            int num7 = 0;
            foreach (DidimoImporter.DidimoData.Group group in didimo.groups)
            {
              List<(int, CharacterGroup.Meta, IReadOnlyList<int>)> valueTupleList = new List<(int, CharacterGroup.Meta, IReadOnlyList<int>)>();
              foreach (DidimoImporter.DidimoData.Character character in group.characters)
              {
                List<int> intList = new List<int>();
                foreach (int renderObjectIndex in character.renderObjectIndices)
                {
                  if (!intSet.Contains(renderObjectIndex))
                    intList.Add(count1 + renderObjectIndex);
                }
                CharacterGroup.Meta meta = new CharacterGroup.Meta()
                {
                  shapeWeights = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.IndexWeight8, CharacterGroup.IndexWeight8>(character.meta.shapeWeights),
                  textureWeights = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.IndexWeight8, CharacterGroup.IndexWeight8>(character.meta.textureWeights),
                  overlayWeights = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.IndexWeight8, CharacterGroup.IndexWeight8>(character.meta.overlayWeights),
                  maskWeights = AssetImportPipeline.CastStruct<DidimoImporter.DidimoData.IndexWeight8, CharacterGroup.IndexWeight8>(character.meta.maskWeights)
                };
                valueTupleList.Add((character.styleIndex, meta, (IReadOnlyList<int>) intList));
              }
              groups.Add((!string.IsNullOrEmpty(group.name) ? group.name : string.Format("Group#{0}", (object) num7++), (IReadOnlyList<(int, CharacterGroup.Meta, IReadOnlyList<int>)>) valueTupleList));
            }
            AssetImportPipeline.log.InfoFormat(string.Format("textureOverlays.Count: {0}", (object) didimo.textureOverlays.Count));
            for (int index = 0; index < didimo.textureOverlays.Count; ++index)
              AssetImportPipeline.log.InfoFormat(string.Format("textureOverlays[{0}]: {1}", (object) index, (object) didimo.textureOverlays[index]));
          }
        }
        postImportOperations = (Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory>) ((sourcePath, importMode, report2, VTMaterials, prefabFactory) =>
        {
          Colossal.IO.AssetDatabase.AssetDatabase.global.UnloadAllAssets();
          List<RenderPrefab> prefabs = new List<RenderPrefab>();
          foreach ((List<Colossal.AssetPipeline.LOD>, string) valueTuple in allRenderGroups)
          {
            (RenderPrefab prefab, Report.Prefab report) tuple = AssetImportPipeline.CreateRenderPrefab(settings, sourcePath, (IReadOnlyList<Colossal.AssetPipeline.LOD>) valueTuple.Item1, importMode, report2, VTMaterials, prefabFactory)[0];
            if (!string.IsNullOrEmpty(valueTuple.Item2))
            {
              CharacterProperties component = tuple.prefab.AddOrGetComponent<CharacterProperties>();
              component.m_BodyParts = (CharacterProperties.BodyPart) 0;
              foreach (string p1 in valueTuple.Item2.Split(',', StringSplitOptions.None))
              {
                CharacterProperties.BodyPart result;
                if (Enum.TryParse<CharacterProperties.BodyPart>(p1, out result))
                  component.m_BodyParts |= result;
                else
                  AssetImportPipeline.log.WarnFormat("Unknown bodypart type ({0}).", (object) p1);
              }
            }
            prefabs.Add(tuple.prefab);
          }
          foreach ((List<Colossal.AssetPipeline.LOD>, string) valueTuple in allRenderGroups)
            AssetImportPipeline.DisposeLODs((IReadOnlyList<Colossal.AssetPipeline.LOD>) valueTuple.Item1);
          System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.IO.AssetDatabase.AnimationAsset> overrideSafeGuard = new System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.IO.AssetDatabase.AnimationAsset>();
          foreach ((string, IReadOnlyList<(int, CharacterGroup.Meta, IReadOnlyList<int>)>) valueTuple3 in groups)
          {
            CharacterGroup prefab = AssetImportPipeline.CreatePrefab<CharacterGroup>("Group", sourcePath, valueTuple3.Item1, 0, prefabFactory);
            prefab.m_Characters = new CharacterGroup.Character[valueTuple3.Item2.Count];
            for (int index2 = 0; index2 < valueTuple3.Item2.Count; ++index2)
            {
              (int, CharacterGroup.Meta, IReadOnlyList<int>) valueTuple4 = valueTuple3.Item2[index2];
              CharacterGroup.Character character = new CharacterGroup.Character();
              int count = valueTuple4.Item3.Count;
              character.m_MeshPrefabs = new RenderPrefab[count];
              character.m_Style = AssetImportPipeline.CreateStylePrefab(characterStyles[valueTuple4.Item1], sourcePath, overrideSafeGuard, prefabs, prefabFactory);
              character.m_Meta = valueTuple4.Item2;
              for (int index3 = 0; index3 < count; ++index3)
                character.m_MeshPrefabs[index3] = prefabs[valueTuple4.Item3[index3]];
              prefab.m_Characters[index2] = character;
            }
          }
        });
      }
    }

    private static TTo CastStruct<TFrom, TTo>(TFrom s)
      where TFrom : struct
      where TTo : struct
    {
      TFrom from = s;
      return UnsafeUtility.As<TFrom, TTo>(ref from);
    }

    private static unsafe NativeArray<byte> FromManagedArray<T>(T[] array) where T : unmanaged
    {
      int num = array.Length * UnsafeUtility.SizeOf<T>();
      NativeArray<byte> nativeArray = new NativeArray<byte>(num, Allocator.Persistent);
      fixed (T* source = array)
        UnsafeUtility.MemCpy(nativeArray.GetUnsafePtr<byte>(), (void*) source, (long) num);
      return nativeArray;
    }

    private static CharacterStyle CreateStylePrefab(
      (string name, IReadOnlyList<(Colossal.Animations.Animation data, Colossal.Animations.BoneHierarchy boneHierarchy, string shortName, int targetRenderGroup, Colossal.Hash128 hash)> animations, int boneCount, int shapeCount) style,
      string sourcePath,
      System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.IO.AssetDatabase.AnimationAsset> overrideSafeGuard,
      List<RenderPrefab> prefabs,
      IPrefabFactory prefabFactory = null)
    {
      CharacterStyle prefab = AssetImportPipeline.CreatePrefab<CharacterStyle>("Style", sourcePath, style.name, 0, prefabFactory);
      int infoIndex = 0;
      System.Collections.Generic.Dictionary<Colossal.Hash128, CharacterStyle.AnimationInfo> dictionary1 = new System.Collections.Generic.Dictionary<Colossal.Hash128, CharacterStyle.AnimationInfo>();
      if (prefab.m_Animations != null)
      {
        foreach (CharacterStyle.AnimationInfo animation in prefab.m_Animations)
          dictionary1[animation.animationAsset.guid] = animation;
      }
      prefab.m_ShapeCount = style.shapeCount;
      prefab.m_BoneCount = style.boneCount;
      prefab.m_Animations = new CharacterStyle.AnimationInfo[style.animations.Count];
      System.Collections.Generic.Dictionary<int, Colossal.Animations.Animation> dictionary2 = new System.Collections.Generic.Dictionary<int, Colossal.Animations.Animation>();
      foreach ((Colossal.Animations.Animation data, Colossal.Animations.BoneHierarchy boneHierarchy, string shortName, int targetRenderGroup, Colossal.Hash128 hash) tuple in (IEnumerable<(Colossal.Animations.Animation data, Colossal.Animations.BoneHierarchy boneHierarchy, string shortName, int targetRenderGroup, Colossal.Hash128 hash)>) style.animations)
      {
        if (tuple.data.type == Colossal.Animations.AnimationType.RestPose)
        {
          int key = tuple.data.layer == Colossal.Animations.AnimationLayer.PropLayer ? tuple.targetRenderGroup : -1;
          dictionary2[key] = tuple.data;
        }
      }
      foreach ((Colossal.Animations.Animation data, Colossal.Animations.BoneHierarchy boneHierarchy, string shortName, int targetRenderGroup, Colossal.Hash128 hash) tuple in (IEnumerable<(Colossal.Animations.Animation data, Colossal.Animations.BoneHierarchy boneHierarchy, string shortName, int targetRenderGroup, Colossal.Hash128 hash)>) style.animations)
      {
        using (AssetImportPipeline.s_Progress.ScopedThreadDescription("Processing animation " + tuple.data.name))
        {
          if (tuple.data.layer != Colossal.Animations.AnimationLayer.PropLayer)
            Assert.AreEqual(tuple.boneHierarchy.hierarchyParentIndices.Length, style.boneCount);
          Colossal.Animations.AnimationClip animation = new Colossal.Animations.AnimationClip()
          {
            m_BoneHierarchy = tuple.boneHierarchy,
            m_Animation = tuple.data
          };
          bool flag = false;
          Colossal.IO.AssetDatabase.AnimationAsset animationAsset1;
          if (!overrideSafeGuard.TryGetValue(tuple.hash, out animationAsset1))
          {
            using (Colossal.IO.AssetDatabase.AnimationAsset animationAsset2 = Colossal.IO.AssetDatabase.AssetDatabase.game.AddAsset(AssetDataPath.Create("StreamingData~", string.Format("{0}_{1}", (object) animation.name, (object) HashUtils.GetHash(animation, sourcePath))), animation))
            {
              animationAsset2.Save(false);
              animationAsset1 = animationAsset2;
              overrideSafeGuard.Add(tuple.hash, animationAsset2);
              flag = true;
            }
          }
          CharacterStyle.AnimationInfo animationInfo;
          if (dictionary1.TryGetValue(animationAsset1.guid, out animationInfo))
          {
            prefab.m_Animations[infoIndex] = animationInfo;
          }
          else
          {
            prefab.m_Animations[infoIndex] = new CharacterStyle.AnimationInfo();
            prefab.m_Animations[infoIndex].animationAsset = (AssetReference<Colossal.IO.AssetDatabase.AnimationAsset>) animationAsset1;
          }
          prefab.m_Animations[infoIndex].name = tuple.shortName;
          prefab.m_Animations[infoIndex].type = tuple.data.type;
          prefab.m_Animations[infoIndex].layer = tuple.data.layer;
          prefab.m_Animations[infoIndex].frameCount = tuple.data.frameCount;
          prefab.m_Animations[infoIndex].frameRate = tuple.data.frameRate;
          if (tuple.targetRenderGroup != -1)
            prefab.m_Animations[infoIndex].target = prefabs[tuple.targetRenderGroup];
          if (flag)
          {
            if (tuple.data.layer == Colossal.Animations.AnimationLayer.BodyLayer || tuple.data.layer == Colossal.Animations.AnimationLayer.PropLayer)
            {
              int key = tuple.data.layer == Colossal.Animations.AnimationLayer.PropLayer ? tuple.targetRenderGroup : -1;
              prefab.CalculateRootMotion(tuple.boneHierarchy, tuple.data, dictionary2[key], infoIndex);
            }
            else
            {
              prefab.m_Animations[infoIndex].rootMotionBone = -1;
              prefab.m_Animations[infoIndex].rootMotion = (CharacterStyle.AnimationMotion[]) null;
            }
          }
          ++infoIndex;
        }
      }
      return prefab;
    }

    private static unsafe NativeArray<byte> FromManagedArray<T>(
      T[] array,
      int offset,
      int elementSize,
      int countPerElement)
      where T : unmanaged
    {
      int sourceStride = UnsafeUtility.SizeOf<T>() / countPerElement;
      int count = array.Length * countPerElement;
      NativeArray<byte> nativeArray = new NativeArray<byte>(count * elementSize, Allocator.Persistent);
      T[] objArray;
      void* source = (void*) (((objArray = array) == null || objArray.Length == 0 ? (IntPtr) (void*) null : (IntPtr) &objArray[0]) + offset);
      UnsafeUtility.MemCpyStride(nativeArray.GetUnsafePtr<byte>(), elementSize, source, sourceStride, elementSize, count);
      objArray = (T[]) null;
      return nativeArray;
    }

    private static void SetupEmissiveComponent(RenderPrefab meshPrefab, Colossal.AssetPipeline.LOD lod)
    {
      List<EmissiveProperties.SingleLightMapping> singleLightMappingList1 = new List<EmissiveProperties.SingleLightMapping>();
      int num = 0;
      foreach (Surface surface in lod.surfaces)
      {
        if (surface.HasProperty("_EmissiveColorMap"))
        {
          List<EmissiveProperties.SingleLightMapping> singleLightMappingList2 = singleLightMappingList1;
          EmissiveProperties.SingleLightMapping singleLightMapping = new EmissiveProperties.SingleLightMapping();
          singleLightMapping.purpose = surface.name.Contains("Neon") ? EmissiveProperties.Purpose.NeonSign : EmissiveProperties.Purpose.DecorativeLight;
          singleLightMapping.intensity = 5f;
          singleLightMapping.materialId = num++;
          singleLightMappingList2.Add(singleLightMapping);
        }
      }
      if (singleLightMappingList1.Count <= 0)
        return;
      EmissiveProperties component;
      if (!meshPrefab.TryGet<EmissiveProperties>(out component))
      {
        AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {1} was missing EmissiveProperties. {0} single lights found. Please set up correctly...", (object) singleLightMappingList1.Count, (object) meshPrefab.name);
        component = meshPrefab.AddComponent<EmissiveProperties>();
        component.m_SingleLights = singleLightMappingList1;
      }
      else
      {
        if (component.m_SingleLights.Count == singleLightMappingList1.Count)
          return;
        AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {2} has an EmissiveProperties but the asset contains a different lightCount than set. Expected: {0} Found: {1}. Please set up correctly...", (object) singleLightMappingList1.Count, (object) component.m_SingleLights.Count, (object) meshPrefab.name);
      }
    }

    private static void ImportTextures(
      Colossal.AssetPipeline.Settings settings,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      (Report.ImportStep step, Report.Asset asset) report)
    {
      AssetImportPipeline.ImportTextures(settings, relativeRootPath, assetGroup, (Func<Colossal.AssetPipeline.TextureAsset, bool>) null, report);
    }

    private static void ImportTextures(
      Colossal.AssetPipeline.Settings settings,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      Func<Colossal.AssetPipeline.TextureAsset, bool> predicate,
      (Report.ImportStep step, Report.Asset asset) report)
    {
      using (AssetImportPipeline.s_ProfImportTextures.Auto())
      {
        using (Report.ImportStep texturesReport = report.step.AddImportStep("Import Textures"))
        {
          ParallelOptions parallelOptions = new ParallelOptions()
          {
            MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
          };
          int failures = 0;
          int totalTif = 0;
          int totalPng = 0;
          int total = 0;
          int totalNpot = 0;
          int totalNon8Bpp = 0;
          long totalFileSize = 0;
          long totalWidth = 0;
          long totalHeight = 0;
          using (PerformanceCounter.Start((Action<TimeSpan>) (t =>
          {
            double b = (double) totalFileSize * 1.0 / (double) total;
            double num1 = (double) totalWidth * 1.0 / (double) total;
            double num2 = (double) totalHeight * 1.0 / (double) total;
            if (total == 0)
              AssetImportPipeline.log.Info((object) string.Format("No textures processed. All textures in this group were already loaded. {0:F3}", (object) t.TotalSeconds));
            else
              AssetImportPipeline.log.Info((object) string.Format("Completed {0} textures import in {1:F3}s. Errors {2}, png {3}, tif {4}, NPOT {5}; 16bpp {6}. Total size {7}, avg size {8}, {9:F0}x{10:F0}", (object) total, (object) t.TotalSeconds, (object) failures, (object) totalPng, (object) totalTif, (object) totalNpot, (object) totalNon8Bpp, (object) FormatUtils.FormatBytes(totalFileSize), (object) FormatUtils.FormatBytes((long) b), (object) num1, (object) num2));
          })))
            Parallel.ForEach<Colossal.AssetPipeline.TextureAsset>(assetGroup.FilterBy<Colossal.AssetPipeline.TextureAsset>(predicate), parallelOptions, (Action<Colossal.AssetPipeline.TextureAsset, ParallelLoopState, long>) ((asset, state, index) =>
            {
              if (asset.instance != null)
                return;
              Report.FileReport report1 = report.asset.AddFile((IAsset) asset);
              using (AssetImportPipeline.s_Progress.ScopedThreadDescription("Importing " + asset.fileName))
              {
                if (AssetImportPipeline.s_Progress.shouldCancel)
                  state.Stop();
                try
                {
                  ISettings importSettings = settings.GetImportSettings(asset.fileName, (IAssetImporter) asset.importer, (ReportBase) report1);
                  using (texturesReport.AddImportStep("Asset import"))
                    asset.instance = asset.importer.Import(importSettings, asset.path, report1);
                  asset.instance.sourceAsset = (IAsset) asset;
                  Interlocked.Increment(ref total);
                  if (asset.instance == null)
                    return;
                  foreach (ITexturePostProcessor texturePostProcessor in (IEnumerable<ITexturePostProcessor>) PostProcessorCache.GetTexturePostProcessors())
                  {
                    ISettings settings1;
                    if (settings.GetPostProcessSettings(asset.fileName, (ISettingable) texturePostProcessor, (ReportBase) report1, out settings1))
                    {
                      Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings1, settings);
                      if (texturePostProcessor.ShouldExecute(context, asset))
                      {
                        using (texturesReport.AddImportStep("Execute " + PPUtils.GetPostProcessorString(texturePostProcessor.GetType()) + " Post Processors"))
                          texturePostProcessor.Execute(context, asset, report1);
                      }
                    }
                  }
                  Interlocked.Add(ref totalFileSize, asset.instance.fileDataLength);
                  Interlocked.Add(ref totalWidth, (long) asset.instance.info.width);
                  Interlocked.Add(ref totalHeight, (long) asset.instance.info.height);
                  if (!Mathf.IsPowerOfTwo(asset.instance.info.width) || !Mathf.IsPowerOfTwo(asset.instance.info.height))
                    Interlocked.Increment(ref totalNpot);
                  if (asset.instance.info.bpp != 8)
                    Interlocked.Increment(ref totalNon8Bpp);
                  if (asset.instance.info.fileFormat == NativeTextures.ImageFileFormat.PNG)
                    Interlocked.Increment(ref totalPng);
                  if (asset.instance.info.fileFormat != NativeTextures.ImageFileFormat.TIFF)
                    return;
                  Interlocked.Increment(ref totalTif);
                }
                catch (Exception ex)
                {
                  Interlocked.Increment(ref failures);
                  AssetImportPipeline.log.Error(ex, (object) ("Error importing " + asset.name + ". Skipped..."));
                }
              }
            }));
        }
      }
    }

    private static void CreateGeometriesAndSurfaces(
      Colossal.AssetPipeline.Settings settings,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<IAsset> assetGroup,
      out Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory> postImportOperations,
      (Report parent, Report.Asset asset) report1)
    {
      using (AssetImportPipeline.s_ProfCreateGeomsSurfaces.Auto())
      {
        using (Report.ImportStep importStep = report1.parent.AddImportStep("Create Geometry and Surfaces"))
        {
          System.Collections.Generic.Dictionary<ModelAsset, List<List<(ModelImporter.Model, Surface)>>> dictionary1 = new System.Collections.Generic.Dictionary<ModelAsset, List<List<(ModelImporter.Model, Surface)>>>();
          foreach (ModelAsset modelAsset in assetGroup.FilterBy<ModelAsset>())
          {
            int lod = modelAsset.lod;
            List<List<(ModelImporter.Model, Surface)>> valueTupleListList;
            if (!dictionary1.TryGetValue(modelAsset, out valueTupleListList))
            {
              valueTupleListList = new List<List<(ModelImporter.Model, Surface)>>();
              dictionary1.Add(modelAsset, valueTupleListList);
            }
            while (valueTupleListList.Count <= lod)
              valueTupleListList.Add(new List<(ModelImporter.Model, Surface)>());
            List<(ModelImporter.Model, Surface)> valueTupleList = valueTupleListList[lod];
            for (int index = 0; index < modelAsset.instance.Count; ++index)
            {
              ModelImporter.Model model = modelAsset.instance[index];
              Surface surface = new Surface(model.name);
              Report.AssetData assetData = report1.parent.AddAssetData(surface.name, typeof (Surface));
              foreach (Colossal.AssetPipeline.TextureAsset textureAsset in assetGroup.FilterBy<Colossal.AssetPipeline.TextureAsset>())
              {
                string bakingTextureProperty = Constants.Material.GetBakingTextureProperty(textureAsset.suffix);
                if (bakingTextureProperty != null && !surface.HasBakingTexture(bakingTextureProperty) && textureAsset.material == modelAsset.material)
                  surface.AddBakingTexture(bakingTextureProperty, textureAsset.instance);
                string shaderProperty = Constants.Material.GetShaderProperty(textureAsset.suffix, report1.asset);
                if (shaderProperty != null && (!surface.HasProperty(shaderProperty) || textureAsset.assetName == modelAsset.assetName && textureAsset.module == modelAsset.module) && textureAsset.material == modelAsset.material && textureAsset.lod == modelAsset.lod)
                {
                  surface.AddProperty(shaderProperty, (TextureImporter.ITexture) textureAsset.instance);
                  report1.parent.AddAssetData(Path.GetFileNameWithoutExtension(textureAsset.fileName), typeof (TextureImporter.Texture)).AddFile((IAsset) textureAsset);
                }
              }
              if (index == 0 && modelAsset.name == settings.mainAsset)
                valueTupleList.Insert(0, (model, surface));
              else
                valueTupleList.Add((model, surface));
              Report.FileReport fileReport = report1.parent.GetFileReport((IAsset) modelAsset);
              foreach (IModelSurfacePostProcessor surfacePostProcessor in (IEnumerable<IModelSurfacePostProcessor>) PostProcessorCache.GetModelSurfacePostProcessors())
              {
                string name = modelAsset.name;
                if (modelAsset.instance.Count > 1)
                  name += string.Format("#{0}", (object) index);
                ISettings settings1;
                if (settings.GetPostProcessSettings(name, (ISettingable) surfacePostProcessor, (ReportBase) fileReport, out settings1))
                {
                  Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings1, settings);
                  if (surfacePostProcessor.ShouldExecute(context, modelAsset, index, surface))
                  {
                    using (importStep.AddImportStep("Execute " + PPUtils.GetPostProcessorString(surfacePostProcessor.GetType()) + " Post Processors"))
                      surfacePostProcessor.Execute(context, modelAsset, index, surface, (report1.parent, report1.asset, fileReport, assetData));
                  }
                }
              }
            }
          }
          List<List<Colossal.AssetPipeline.LOD>> assets = new List<List<Colossal.AssetPipeline.LOD>>(dictionary1.Count);
          if (dictionary1.Count > 0)
          {
            foreach (List<List<(ModelImporter.Model, Surface)>> valueTupleListList in dictionary1.Values)
            {
              List<Colossal.AssetPipeline.LOD> asset = new List<Colossal.AssetPipeline.LOD>();
              for (int index1 = 0; index1 < valueTupleListList.Count; ++index1)
              {
                List<(ModelImporter.Model, Surface)> source = valueTupleListList[index1];
                if (source.Count != 0)
                {
                  Geometry geometry = new Geometry(source.Select<(ModelImporter.Model, Surface), ModelImporter.Model>((Func<(ModelImporter.Model, Surface), ModelImporter.Model>) (t => t.model)).ToArray<ModelImporter.Model>());
                  Surface[] array = source.Select<(ModelImporter.Model, Surface), Surface>((Func<(ModelImporter.Model, Surface), Surface>) (t => t.surface)).ToArray<Surface>();
                  if (asset.Count > index1)
                  {
                    asset[index1] = new Colossal.AssetPipeline.LOD(geometry, array, index1);
                    report1.asset.AddWarning(string.Format("LOD {0} already exist and was replaced by last imported.", (object) index1));
                  }
                  else
                    asset.Add(new Colossal.AssetPipeline.LOD(geometry, array, index1));
                  Report.AssetData assetData = report1.parent.AddAssetData(geometry.name, typeof (Geometry));
                  assetData.AddFiles(source.Select<(ModelImporter.Model, Surface), IAsset>((Func<(ModelImporter.Model, Surface), IAsset>) (t => t.model.sourceAsset)));
                  Report.AssetData[] assetDataArray = new Report.AssetData[array.Length];
                  int index2 = 0;
                  foreach (Surface surface in array)
                  {
                    assetDataArray[index2] = report1.parent.AddAssetData(surface.name, typeof (Surface));
                    assetDataArray[index2++].AddFiles(surface.textures.Values.Select<TextureImporter.ITexture, IAsset>((Func<TextureImporter.ITexture, IAsset>) (t => t.sourceAsset)));
                  }
                  foreach (IGeometryPostProcessor geometryPostProcessor in (IEnumerable<IGeometryPostProcessor>) PostProcessorCache.GetGeometryPostProcessors())
                  {
                    ISettings settings2;
                    if (settings.GetPostProcessSettings(geometry.name, (ISettingable) geometryPostProcessor, (ReportBase) report1.asset, out settings2))
                    {
                      Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings2, settings);
                      if (geometryPostProcessor.ShouldExecute(context, asset))
                      {
                        using (importStep.AddImportStep("Execute " + PPUtils.GetPostProcessorString(geometryPostProcessor.GetType()) + " Post Processors"))
                          geometryPostProcessor.Execute(context, asset, (report1.parent, report1.asset, assetData, assetDataArray));
                      }
                    }
                  }
                }
              }
              assets.Add(asset);
            }
          }
          else
          {
            List<Colossal.AssetPipeline.LOD> lodList = new List<Colossal.AssetPipeline.LOD>();
            System.Collections.Generic.Dictionary<string, List<Colossal.AssetPipeline.TextureAsset>> dictionary2 = assetGroup.FilterBy<Colossal.AssetPipeline.TextureAsset>().GroupBy<Colossal.AssetPipeline.TextureAsset, string>((Func<Colossal.AssetPipeline.TextureAsset, string>) (t => t.material)).ToDictionary<IGrouping<string, Colossal.AssetPipeline.TextureAsset>, string, List<Colossal.AssetPipeline.TextureAsset>>((Func<IGrouping<string, Colossal.AssetPipeline.TextureAsset>, string>) (g => g.Key), (Func<IGrouping<string, Colossal.AssetPipeline.TextureAsset>, List<Colossal.AssetPipeline.TextureAsset>>) (g => g.ToList<Colossal.AssetPipeline.TextureAsset>()));
            List<Surface> surfaceList = new List<Surface>();
            foreach (KeyValuePair<string, List<Colossal.AssetPipeline.TextureAsset>> keyValuePair in dictionary2)
            {
              string name = assetGroup.name;
              if (!string.IsNullOrEmpty(keyValuePair.Key))
                name += keyValuePair.Key;
              Surface surface = new Surface(name);
              Report.AssetData report = report1.parent.AddAssetData(surface.name, typeof (Surface));
              foreach (Colossal.AssetPipeline.TextureAsset textureAsset in keyValuePair.Value)
              {
                string bakingTextureProperty = Constants.Material.GetBakingTextureProperty(textureAsset.suffix);
                if (bakingTextureProperty != null && !surface.HasBakingTexture(bakingTextureProperty))
                  surface.AddBakingTexture(bakingTextureProperty, textureAsset.instance);
                string shaderProperty = Constants.Material.GetShaderProperty(textureAsset.suffix, report1.asset);
                if (shaderProperty != null && !surface.HasProperty(shaderProperty) && textureAsset.material == keyValuePair.Key)
                {
                  surface.AddProperty(shaderProperty, (TextureImporter.ITexture) textureAsset.instance);
                  report1.parent.AddAssetData(Path.GetFileNameWithoutExtension(textureAsset.fileName), typeof (TextureImporter.Texture)).AddFile((IAsset) textureAsset);
                }
              }
              foreach (IModelSurfacePostProcessor surfacePostProcessor in (IEnumerable<IModelSurfacePostProcessor>) PostProcessorCache.GetModelSurfacePostProcessors())
              {
                ISettings settings3;
                if (settings.GetPostProcessSettings(name, (ISettingable) surfacePostProcessor, (ReportBase) report, out settings3))
                {
                  Colossal.AssetPipeline.PostProcessors.Context context = new Colossal.AssetPipeline.PostProcessors.Context(AssetImportPipeline.s_MainThreadDispatcher, relativeRootPath, AssetImportPipeline.OnDebugTexture, settings3, settings);
                  if (surfacePostProcessor.ShouldExecute(context, (ModelAsset) null, 0, surface))
                  {
                    using (importStep.AddImportStep("Execute " + PPUtils.GetPostProcessorString(surfacePostProcessor.GetType()) + " Post Processors"))
                      surfacePostProcessor.Execute(context, (ModelAsset) null, 0, surface, (report1.parent, report1.asset, (Report.FileReport) null, report));
                  }
                }
              }
              surfaceList.Add(surface);
            }
            lodList.Add(new Colossal.AssetPipeline.LOD((Geometry) null, surfaceList.ToArray(), 0));
            assets.Add(lodList);
          }
          postImportOperations = (Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory>) ((sourcePath, importMode, report2, VTMaterials, prefabFactory) =>
          {
            AssetImportPipeline.CreateRenderPrefabs(settings, sourcePath, (IReadOnlyList<List<Colossal.AssetPipeline.LOD>>) assets, importMode, report2, VTMaterials, prefabFactory);
            AssetImportPipeline.DisposeLODs((IReadOnlyList<IReadOnlyList<Colossal.AssetPipeline.LOD>>) assets);
          });
        }
      }
    }

    private static bool ImportAssetGroup(
      string projectRootPath,
      string relativeRootPath,
      SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset> assetGroup,
      out List<List<Colossal.AssetPipeline.LOD>> lods,
      out Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory> postImportOperations,
      Report report,
      Report.Asset assetReport)
    {
      lods = (List<List<Colossal.AssetPipeline.LOD>>) null;
      postImportOperations = (Action<string, ImportMode, Report, HashSet<SurfaceAsset>, IPrefabFactory>) null;
      using (AssetImportPipeline.s_ProfImportAssetGroup.Auto())
      {
        try
        {
          AssetImportPipeline.log.Info((object) ("Start processing " + assetGroup.ToString()));
          using (Report.ImportStep importStep = report.AddImportStep("Import asset group"))
          {
            Colossal.AssetPipeline.Settings settings = AssetImportPipeline.ImportSettings(assetGroup, (importStep, assetReport));
            SourceAssetCollector.AssetGroup<IAsset> groupFromSettings = AssetImportPipeline.CreateAssetGroupFromSettings(projectRootPath, settings, assetGroup, assetReport);
            foreach (IAsset asset in groupFromSettings)
              AssetImportPipeline.log.Verbose((object) string.Format("   {0}", (object) asset));
            if (settings.pipeline == Pipeline.Default)
            {
              AssetImportPipeline.ImportTextures(settings, relativeRootPath, groupFromSettings, (importStep, assetReport));
              AssetImportPipeline.ImportModels(settings, relativeRootPath, groupFromSettings, (importStep, assetReport));
              AssetImportPipeline.CreateGeometriesAndSurfaces(settings, relativeRootPath, groupFromSettings, out postImportOperations, (report, assetReport));
            }
            else if (settings.pipeline == Pipeline.Characters)
            {
              AssetImportPipeline.ImportDidimo(settings, groupFromSettings, (importStep, assetReport));
              AssetImportPipeline.CreateDidimoAssets(settings, relativeRootPath, groupFromSettings, out postImportOperations, (report, importStep, assetReport));
            }
          }
          return true;
        }
        catch (Exception ex)
        {
          AssetImportPipeline.log.ErrorFormat(ex, "Error processing {0}.. Skipped!", (object) assetGroup.ToString());
          lods = (List<List<Colossal.AssetPipeline.LOD>>) null;
          return false;
        }
      }
    }

    private static bool IsLODsValid(IReadOnlyList<List<Colossal.AssetPipeline.LOD>> assets)
    {
      if (assets == null || assets.Count == 0)
        return false;
      foreach (List<Colossal.AssetPipeline.LOD> asset in (IEnumerable<List<Colossal.AssetPipeline.LOD>>) assets)
      {
        foreach (Colossal.AssetPipeline.LOD lod in asset)
        {
          if (lod.geometry == null && (lod.surfaces == null || lod.surfaces.Length == 0) || lod.geometry != null && !lod.geometry.isValid || lod.surfaces != null && (lod.surfaces.Length == 0 || ((IEnumerable<Surface>) lod.surfaces).Any<Surface>((Func<Surface, bool>) (surface => !surface.isValid))))
            return false;
        }
      }
      return true;
    }

    private static void DisposeLODs(IReadOnlyList<IReadOnlyList<Colossal.AssetPipeline.LOD>> assets)
    {
      foreach (IReadOnlyList<Colossal.AssetPipeline.LOD> asset in (IEnumerable<IReadOnlyList<Colossal.AssetPipeline.LOD>>) assets)
        AssetImportPipeline.DisposeLODs(asset);
    }

    private static void DisposeLODs(IReadOnlyList<Colossal.AssetPipeline.LOD> assets)
    {
      foreach (Colossal.AssetPipeline.LOD asset in (IEnumerable<Colossal.AssetPipeline.LOD>) assets)
        asset.Dispose();
    }

    private static IReadOnlyList<(RenderPrefab prefab, Report.Prefab report)> CreateRenderPrefab(
      Colossal.AssetPipeline.Settings settings,
      string sourcePath,
      IReadOnlyList<Colossal.AssetPipeline.LOD> asset,
      ImportMode importMode,
      Report report,
      HashSet<SurfaceAsset> VTMaterials,
      IPrefabFactory prefabFactory = null)
    {
      List<(RenderPrefab, Report.Prefab)> meshPrefabs = new List<(RenderPrefab, Report.Prefab)>(asset.Count);
      try
      {
        foreach (Colossal.AssetPipeline.LOD lod in (IEnumerable<Colossal.AssetPipeline.LOD>) asset)
        {
          RenderPrefab renderPrefab = AssetImportPipeline.CreateRenderPrefab(sourcePath, lod.name, lod.level, prefabFactory);
          Report.Prefab report1 = report.AddPrefab(renderPrefab.name);
          meshPrefabs.Add((renderPrefab, report1));
          if (importMode.Has(ImportMode.Geometry))
          {
            Geometry geometry = lod.geometry;
            if (geometry != null)
            {
              using (GeometryAsset asset1 = AssetImportPipeline.targetDatabase.AddAsset(AssetDataPath.Create("StreamingData~", string.Format("{0}_{1}", (object) lod.name, (object) HashUtils.GetHash(lod, sourcePath))), geometry))
              {
                asset1.Save(false);
                report.AddInfoToAsset(lod.name, typeof (Geometry), (Report.IAddressableAsset) asset1);
                renderPrefab.geometryAsset = asset1;
                renderPrefab.bounds = geometry.CalcBounds();
                renderPrefab.surfaceArea = geometry.CalcSurfaceArea();
                renderPrefab.indexCount = geometry.CalcTotalIndices();
                renderPrefab.vertexCount = geometry.CalcTotalVertices();
                renderPrefab.meshCount = geometry.models.Length;
              }
            }
            else if (lod.surfaces != null)
              renderPrefab.meshCount = lod.surfaces.Length;
          }
          if (importMode.Has(ImportMode.Textures))
          {
            Surface[] surfaces = lod.surfaces;
            if (surfaces != null)
            {
              SurfaceAsset[] surfaceAssetArray = new SurfaceAsset[surfaces.Length];
              for (int index = 0; index < surfaces.Length; ++index)
              {
                Surface surface = surfaces[index];
                try
                {
                  AssetImportPipeline.targetDatabase.onAssetDatabaseChanged.Subscribe<Colossal.IO.AssetDatabase.TextureAsset>(new EventDelegate<AssetChangedEventArgs>(OnTextureAdded));
                  using (SurfaceAsset asset2 = AssetImportPipeline.targetDatabase.AddAsset(AssetDataPath.Create("StreamingData~", string.Format("{0}_{1}", (object) surface.name, (object) HashUtils.GetHash(surface, sourcePath))), surface))
                  {
                    asset2.Save(0, false, true, false, (VirtualTexturingConfig) null, (System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>>) null, new int?(), new int?());
                    VTMaterials.Add(asset2);
                    surfaceAssetArray[index] = asset2;
                    report.AddInfoToAsset(surface.name, typeof (Surface), (Report.IAddressableAsset) asset2);
                  }
                }
                finally
                {
                  AssetImportPipeline.targetDatabase.onAssetDatabaseChanged.Unsubscribe(new EventDelegate<AssetChangedEventArgs>(OnTextureAdded));
                }
                if (surface.isImpostor)
                  renderPrefab.isImpostor = true;
              }
              renderPrefab.surfaceAssets = (IEnumerable<SurfaceAsset>) surfaceAssetArray;
            }
          }
          if (settings.pipeline == Pipeline.Default)
            AssetImportPipeline.SetupComponents(settings, renderPrefab, lod, report1);
        }
        switch (settings.pipeline)
        {
          case Pipeline.Default:
          case Pipeline.Characters:
            AssetImportPipeline.SetupLODs(settings, (IReadOnlyList<(RenderPrefab, Report.Prefab)>) meshPrefabs);
            break;
        }
      }
      catch (Exception ex)
      {
        AssetImportPipeline.log.Error(ex, (object) ("Error occured with " + asset[0].name));
      }
      return (IReadOnlyList<(RenderPrefab, Report.Prefab)>) meshPrefabs;

      void OnTextureAdded(AssetChangedEventArgs args)
      {
        Colossal.IO.AssetDatabase.TextureAsset asset = (Colossal.IO.AssetDatabase.TextureAsset) args.asset;
        report.AddInfoToAsset(AssetImportPipeline.GetNameWithoutGUID(asset.name), typeof (TextureImporter.Texture), (Report.IAddressableAsset) asset);
      }
    }

    private static RenderPrefab CreateRenderPrefab(
      string sourcePath,
      string name,
      int lodLevel,
      IPrefabFactory prefabFactory = null)
    {
      return AssetImportPipeline.CreatePrefab<RenderPrefab>("Mesh", sourcePath, name, lodLevel, prefabFactory);
    }

    private static T CreatePrefab<T>(
      string suffix,
      string sourcePath,
      string name,
      int lodLevel,
      IPrefabFactory prefabFactory = null)
      where T : PrefabBase
    {
      string name1 = name + (!string.IsNullOrEmpty(suffix) ? " " + suffix : string.Empty);
      T prefab = prefabFactory != null ? prefabFactory.CreatePrefab<T>(sourcePath, name1, lodLevel) : default (T);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        prefab = ScriptableObject.CreateInstance<T>();
        prefab.name = name1;
      }
      return prefab;
    }

    private static void CreateRenderPrefabs(
      Colossal.AssetPipeline.Settings settings,
      string sourcePath,
      IReadOnlyList<List<Colossal.AssetPipeline.LOD>> assets,
      ImportMode importMode,
      Report report,
      HashSet<SurfaceAsset> VTMaterials,
      IPrefabFactory prefabFactory = null)
    {
      using (AssetImportPipeline.s_ProfPostImport.Auto())
      {
        if (!AssetImportPipeline.IsLODsValid(assets))
        {
          AssetImportPipeline.log.DebugFormat("Result for {0} is not valid and will not be serialized", (object) sourcePath);
        }
        else
        {
          string name = assets[0][0].name;
          try
          {
            using (report.AddImportStep("Perform main thread tasks (Assetdatabase serialization + Prefabs upgrade)"))
            {
              foreach (List<Colossal.AssetPipeline.LOD> asset in (IEnumerable<List<Colossal.AssetPipeline.LOD>>) assets)
                AssetImportPipeline.CreateRenderPrefab(settings, sourcePath, (IReadOnlyList<Colossal.AssetPipeline.LOD>) asset, importMode, report, VTMaterials, prefabFactory);
            }
          }
          catch (Exception ex)
          {
            AssetImportPipeline.log.Error(ex, (object) ("Error post-importing " + name + "."));
            report.AddError("Error post-importing " + name + ": " + ex.Message + ".");
          }
        }
      }
    }

    private static void SetupLODs(
      Colossal.AssetPipeline.Settings settings,
      IReadOnlyList<(RenderPrefab prefab, Report.Prefab report)> meshPrefabs)
    {
      if (meshPrefabs.Count <= 1)
        return;
      RenderPrefab prefab = meshPrefabs[0].prefab;
      ProceduralAnimationProperties component1 = prefab.GetComponent<ProceduralAnimationProperties>();
      ContentPrerequisite component2 = prefab.GetComponent<ContentPrerequisite>();
      LodProperties component3 = prefab.AddOrGetComponent<LodProperties>();
      meshPrefabs[0].report.AddComponent(component3.ToString());
      component3.m_LodMeshes = new RenderPrefab[meshPrefabs.Count - 1];
      for (int index = 1; index < meshPrefabs.Count; ++index)
      {
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          ProceduralAnimationProperties animationProperties = meshPrefabs[index].prefab.AddComponentFrom<ProceduralAnimationProperties>(component1);
          meshPrefabs[index].report.AddComponent(animationProperties.ToString());
        }
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        {
          ContentPrerequisite contentPrerequisite = meshPrefabs[index].prefab.AddComponentFrom<ContentPrerequisite>(component2);
          meshPrefabs[index].report.AddComponent(contentPrerequisite.ToString());
        }
        component3.m_LodMeshes[index - 1] = meshPrefabs[index].prefab;
      }
    }

    private static void SetupComponents(
      Colossal.AssetPipeline.Settings settings,
      RenderPrefab meshPrefab,
      Colossal.AssetPipeline.LOD lod,
      Report.Prefab report)
    {
      if (lod.level != 0)
        return;
      AssetImportPipeline.SetupEmissiveComponent(settings, meshPrefab, lod, report);
      if (!settings.useProceduralAnimation)
        return;
      AssetImportPipeline.SetupProceduralAnimationComponent(settings, meshPrefab, lod, report);
    }

    private static void SetupEmissiveComponent(
      Colossal.AssetPipeline.Settings settings,
      RenderPrefab meshPrefab,
      Colossal.AssetPipeline.LOD lod,
      Report.Prefab report)
    {
      List<EmissiveProperties.MultiLightMapping> multiLightProps = new List<EmissiveProperties.MultiLightMapping>();
      List<EmissiveProperties.SingleLightMapping> singleLightMappingList1 = new List<EmissiveProperties.SingleLightMapping>();
      int num = 0;
      foreach (Surface surface in lod.surfaces)
      {
        if (surface.emissiveLayers.Count == 0)
        {
          if (surface.HasProperty("_EmissiveColorMap"))
          {
            List<EmissiveProperties.SingleLightMapping> singleLightMappingList2 = singleLightMappingList1;
            EmissiveProperties.SingleLightMapping singleLightMapping = new EmissiveProperties.SingleLightMapping();
            singleLightMapping.purpose = surface.name.Contains("Neon") ? EmissiveProperties.Purpose.NeonSign : EmissiveProperties.Purpose.DecorativeLight;
            singleLightMapping.intensity = 5f;
            singleLightMapping.materialId = num++;
            singleLightMappingList2.Add(singleLightMapping);
          }
        }
        else
        {
          foreach (Surface.EmissiveLayer emissiveLayer in surface.emissiveLayers)
          {
            List<EmissiveProperties.MultiLightMapping> multiLightMappingList = multiLightProps;
            EmissiveProperties.MultiLightMapping multiLightMapping = new EmissiveProperties.MultiLightMapping();
            multiLightMapping.intensity = emissiveLayer.intensity;
            multiLightMapping.luminance = emissiveLayer.luminance;
            multiLightMapping.color = emissiveLayer.color;
            multiLightMapping.layerId = emissiveLayer.layerId;
            multiLightMapping.purpose = EmissiveProperties.Purpose.None;
            multiLightMapping.colorOff = Color.black;
            multiLightMapping.animationIndex = -1;
            multiLightMapping.responseTime = 0.0f;
            multiLightMappingList.Add(multiLightMapping);
          }
        }
      }
      if (singleLightMappingList1.Count > 0)
      {
        EmissiveProperties component;
        if (!meshPrefab.TryGet<EmissiveProperties>(out component))
        {
          component = meshPrefab.AddComponent<EmissiveProperties>();
          component.m_SingleLights = singleLightMappingList1;
          report.AddComponent(component.ToString()).AddMessage(string.Format("Missing EmissiveProperties. {0} single lights found. Please set up correctly...", (object) singleLightMappingList1.Count));
          AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {1} was missing EmissiveProperties. {0} single lights found. Please set up correctly...", (object) singleLightMappingList1.Count, (object) meshPrefab.name);
        }
        else if (component.m_SingleLights.Count != singleLightMappingList1.Count)
        {
          report.AddComponent(component.ToString()).AddMessage(string.Format("EmissiveProperties already added but the asset contains a different lightCount than set. Expected: {0} Found: {1}. Please set up correctly...", (object) singleLightMappingList1.Count, (object) component.m_SingleLights.Count));
          AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {2} has an EmissiveProperties but the asset contains a different lightCount than set. Expected: {0} Found: {1}. Please set up correctly...", (object) singleLightMappingList1.Count, (object) component.m_SingleLights.Count, (object) meshPrefab.name);
        }
      }
      if (multiLightProps.Count <= 0)
        return;
      EmissiveProperties component1;
      if (!meshPrefab.TryGet<EmissiveProperties>(out component1))
      {
        component1 = meshPrefab.AddComponent<EmissiveProperties>();
        component1.m_MultiLights = multiLightProps;
        report.AddComponent(component1.ToString()).AddMessage(string.Format("Missing EmissiveProperties. {0} light layers found. Please set up correctly...", (object) multiLightProps.Count));
        AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {1} was missing EmissiveProperties. {0} light layers found. Please set up correctly...", (object) multiLightProps.Count, (object) meshPrefab.name);
      }
      else
      {
        if (component1.m_MultiLights.Count != multiLightProps.Count)
        {
          report.AddComponent(component1.ToString()).AddWarning(string.Format("EmissiveProperties already added but the asset contains a different light layer count than set. Expected: {0} Found: {1}. Please set up correctly...", (object) singleLightMappingList1.Count, (object) component1.m_MultiLights.Count));
          AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {2} has an EmissiveProperties but the asset contains a different light layer count than set. Expected: {0} Found: {1}. Please set up correctly...", (object) singleLightMappingList1.Count, (object) component1.m_MultiLights.Count, (object) meshPrefab.name);
        }
        for (int i = 0; i < multiLightProps.Count; ++i)
        {
          EmissiveProperties.MultiLightMapping multiLightMapping = component1.m_MultiLights.Find((Predicate<EmissiveProperties.MultiLightMapping>) (x => x.layerId == multiLightProps[i].layerId));
          if (multiLightMapping != null)
          {
            multiLightProps[i].purpose = multiLightMapping.purpose;
            multiLightProps[i].color = multiLightMapping.color;
            multiLightProps[i].colorOff = multiLightMapping.colorOff;
            multiLightProps[i].animationIndex = multiLightMapping.animationIndex;
            multiLightProps[i].responseTime = multiLightMapping.responseTime;
          }
        }
        component1.m_MultiLights = multiLightProps;
      }
    }

    private static bool GetSkinningInfo(
      ModelImporter.Model model,
      out ModelImporter.Model.BoneInfo[] bones,
      Report.Prefab report)
    {
      bones = model.bones;
      if (model.HasAttribute(VertexAttribute.BlendIndices))
      {
        if (model.rootBoneIndex == -1)
        {
          report.AddWarning(model.name + " is missing root bone");
          return false;
        }
        if (model.bones == null)
        {
          report.AddWarning(model.name + " is missing bind poses");
          return false;
        }
        if (model.HasAttribute(VertexAttribute.BlendWeight) || model.GetAttributeData(VertexAttribute.BlendIndices).dimension == 1)
          return true;
        report.AddWarning(model.name + " has BlendIndices but no BlendWeight. Assuming rigid skinning..");
        return false;
      }
      if (model.HasAttribute(VertexAttribute.BlendWeight))
        report.AddWarning(model.name + " has BlendWeight but is missing BlendIndices");
      return false;
    }

    private static string GetUniqueString(
      string input,
      int currentIndex,
      ProceduralAnimationProperties.BoneInfo[] array)
    {
      int num = 0;
      for (int index = 0; index < currentIndex; ++index)
      {
        if (array[index].name.StartsWith(input))
          ++num;
      }
      return num == 0 ? input : string.Format("{0} {1}", (object) input, (object) num);
    }

    private static void SetupProceduralAnimationComponent(
      Colossal.AssetPipeline.Settings settings,
      RenderPrefab meshPrefab,
      Colossal.AssetPipeline.LOD lod,
      Report.Prefab report)
    {
      ModelImporter.Model.BoneInfo[] bones1;
      if (lod.geometry == null || !AssetImportPipeline.GetSkinningInfo(lod.geometry.models[0], out bones1, report))
        return;
      ProceduralAnimationProperties component;
      if (!meshPrefab.TryGet<ProceduralAnimationProperties>(out component))
      {
        AssetImportPipeline.log.WarnFormat((UnityEngine.Object) meshPrefab, "Mesh prefab {0} was missing ProceduralAnimationProperties. Please set up correctly...", (object) meshPrefab.name);
        component = meshPrefab.AddComponent<ProceduralAnimationProperties>();
        report.AddComponent(component.ToString());
      }
      ProceduralAnimationProperties.BoneInfo[] bones = new ProceduralAnimationProperties.BoneInfo[bones1.Length];
      for (int i = 0; i < bones1.Length; ++i)
      {
        bones[i] = new ProceduralAnimationProperties.BoneInfo()
        {
          name = AssetImportPipeline.GetUniqueString(bones1[i].name, i, bones),
          position = bones1[i].localPosition,
          rotation = bones1[i].localRotation,
          scale = bones1[i].localScale,
          parentId = bones1[i].parentIndex,
          bindPose = bones1[i].bindPose
        };
        if (component.m_Bones != null)
        {
          ProceduralAnimationProperties.BoneInfo boneInfo = System.Array.Find<ProceduralAnimationProperties.BoneInfo>(component.m_Bones, (Predicate<ProceduralAnimationProperties.BoneInfo>) (x => x.name == bones[i].name));
          if (boneInfo != null)
          {
            bones[i].m_Acceleration = boneInfo.m_Acceleration;
            bones[i].m_Speed = boneInfo.m_Speed;
            bones[i].m_ConnectionID = boneInfo.m_ConnectionID;
            bones[i].m_Type = boneInfo.m_Type;
          }
        }
      }
      component.m_Bones = bones;
    }

    private static Colossal.Json.Variant ToJsonSchema(object obj)
    {
      return AssetImportPipeline.ToJsonSchema(obj.GetType(), (Colossal.Json.Variant) null);
    }

    private static Colossal.Json.Variant ToJsonSchema(System.Type type, Colossal.Json.Variant previous = null)
    {
      Colossal.Json.Variant variant = previous ?? (Colossal.Json.Variant) new ProxyObject();
      string jsonType = AssetImportPipeline.ToJsonType(type);
      variant[nameof (type)] = JSON.Load(jsonType);
      ProxyObject properties = new ProxyObject();
      type.ForEachField(EncodeOptions.None, (Colossal.Json.Extensions.MemberDelegate<FieldInfo>) ((fieldInfo, typeHint) =>
      {
        ProxyObject proxyObject = (ProxyObject) JSON.Load("{ " + AssetImportPipeline.ToJsonSchema(fieldInfo) + " }");
        properties[fieldInfo.Name] = (Colossal.Json.Variant) proxyObject;
      }));
      if (typeof (ISettings).IsAssignableFrom(type))
      {
        properties["@type"] = JSON.Load("{ \"$ref\": \"#/definitions/settingsType\" }");
        variant["required"] = (Colossal.Json.Variant) new ProxyArray()
        {
          (Colossal.Json.Variant) new ProxyString("@type")
        };
      }
      variant["properties"] = (Colossal.Json.Variant) properties;
      variant["additionalProperties"] = (Colossal.Json.Variant) new ProxyBoolean(false);
      return properties.Count <= 0 && !(jsonType != "object") ? (Colossal.Json.Variant) null : variant;
    }

    private static string ToJsonType(System.Type type, bool nullable = false)
    {
      if (type == typeof (string))
        return !nullable ? "\"string\"" : "[ \"string\", \"null\" ]";
      if (type.IsEnum)
        return "\"string\"";
      if (type == typeof (uint))
        return "\"nonNegativeInteger\"";
      if (type == typeof (int))
        return "\"integer\"";
      if (type == typeof (float) || type == typeof (double))
        return "\"number\"";
      if (type == typeof (bool))
        return "\"boolean\"";
      return type.IsArray || typeof (IList).IsAssignableFrom(type) ? "\"array\"" : "\"object\"";
    }

    private static string GetSettings()
    {
      string[] array = ImporterCache.GetSupportedExtensions().Select<KeyValuePair<string, List<System.Type>>, ISettings>((Func<KeyValuePair<string, List<System.Type>>, ISettings>) (ext => ImporterCache.GetImporter(ext.Key).GetDefaultSettings())).Concat<ISettings>(PostProcessorCache.GetTexturePostProcessors().Select<ITexturePostProcessor, ISettings>((Func<ITexturePostProcessor, ISettings>) (x => x.GetDefaultSettings()))).Concat<ISettings>(PostProcessorCache.GetModelPostProcessors().Select<IModelPostProcessor, ISettings>((Func<IModelPostProcessor, ISettings>) (x => x.GetDefaultSettings()))).Concat<ISettings>(PostProcessorCache.GetModelSurfacePostProcessors().Select<IModelSurfacePostProcessor, ISettings>((Func<IModelSurfacePostProcessor, ISettings>) (x => x.GetDefaultSettings()))).Concat<ISettings>(PostProcessorCache.GetGeometryPostProcessors().Select<IGeometryPostProcessor, ISettings>((Func<IGeometryPostProcessor, ISettings>) (x => x.GetDefaultSettings()))).Where<ISettings>((Func<ISettings, bool>) (x => x != null)).GroupBy<ISettings, System.Type>((Func<ISettings, System.Type>) (settings => settings.GetType())).Select<IGrouping<System.Type, ISettings>, ISettings>((Func<IGrouping<System.Type, ISettings>, ISettings>) (group => group.First<ISettings>())).Select<ISettings, string>(new Func<ISettings, string>(GetIfThen)).Where<string>((Func<string, bool>) (x => x != null)).ToArray<string>();
      string empty = string.Empty;
      for (int index = 0; index < array.Length; ++index)
      {
        if (index > 0)
          empty += ", \"else\": {";
        empty += array[index];
      }
      string settings1 = empty + ", \"else\": { \"additionalProperties\": false";
      for (int index = 0; index < array.Length; ++index)
        settings1 += "}";
      return settings1;

      static string GetIfThen(ISettings settings)
      {
        Colossal.Json.Variant jsonSchema = AssetImportPipeline.ToJsonSchema((object) settings);
        if (jsonSchema == null)
          return (string) null;
        return "\"if\": { \"properties\": { \"@type\": { \"oneOf\": " + ("[ { \"const\": \"" + settings.GetType().FullName + "\" }, { \"const\": \"" + ReflectionUtils.TypeName(settings.GetType()) + "\" } ]") + " } } }, \"then\": " + jsonSchema.ToJSONString<Colossal.Json.Variant>();
      }
    }

    private static Colossal.Json.Variant GetDefinitions()
    {
      ProxyObject definitions = new ProxyObject();
      definitions["importers"] = JSON.Load("{ \"type\": \"string\", \"enum\": " + ImporterCache.GetSupportedExtensions().Select<KeyValuePair<string, List<System.Type>>, System.Type>((Func<KeyValuePair<string, List<System.Type>>, System.Type>) (ext => ImporterCache.GetImporter(ext.Key).GetType())).Distinct<System.Type>().SelectMany<System.Type, string>((Func<System.Type, IEnumerable<string>>) (t => (IEnumerable<string>) new string[2]
      {
        t.FullName,
        ReflectionUtils.TypeName(t)
      })).ToArray<string>().ToJSONString<string[]>() + " }");
      definitions["settingsType"] = JSON.Load("{ \"type\": \"string\", \"enum\": " + ImporterCache.GetSupportedExtensions().Select<KeyValuePair<string, List<System.Type>>, System.Type>((Func<KeyValuePair<string, List<System.Type>>, System.Type>) (ext => ImporterCache.GetImporter(ext.Key).GetDefaultSettings()?.GetType())).Distinct<System.Type>().Concat<System.Type>(PostProcessorCache.GetTexturePostProcessors().Select<ITexturePostProcessor, System.Type>((Func<ITexturePostProcessor, System.Type>) (x => x.GetDefaultSettings()?.GetType()))).Where<System.Type>((Func<System.Type, bool>) (x => x != (System.Type) null)).Concat<System.Type>(PostProcessorCache.GetModelPostProcessors().Select<IModelPostProcessor, System.Type>((Func<IModelPostProcessor, System.Type>) (x => x.GetDefaultSettings()?.GetType()))).Where<System.Type>((Func<System.Type, bool>) (x => x != (System.Type) null)).Concat<System.Type>(PostProcessorCache.GetModelSurfacePostProcessors().Select<IModelSurfacePostProcessor, System.Type>((Func<IModelSurfacePostProcessor, System.Type>) (x => x.GetDefaultSettings()?.GetType()))).Where<System.Type>((Func<System.Type, bool>) (x => x != (System.Type) null)).Concat<System.Type>(PostProcessorCache.GetGeometryPostProcessors().Select<IGeometryPostProcessor, System.Type>((Func<IGeometryPostProcessor, System.Type>) (x => x.GetDefaultSettings()?.GetType()))).Where<System.Type>((Func<System.Type, bool>) (x => x != (System.Type) null)).SelectMany<System.Type, string>((Func<System.Type, IEnumerable<string>>) (t => (IEnumerable<string>) new string[2]
      {
        t.FullName,
        ReflectionUtils.TypeName(t)
      })).ToArray<string>().ToJSONString<string[]>() + " }");
      return (Colossal.Json.Variant) definitions;
    }

    private static string ToJsonSchema(FieldInfo fieldInfo)
    {
      string name = fieldInfo.Name;
      System.Type fieldType = fieldInfo.FieldType;
      bool nullable = name == "materialTemplate";
      string jsonSchema = "\"type\": " + AssetImportPipeline.ToJsonType(fieldType, nullable);
      if (fieldType.IsEnum)
        return jsonSchema + ", \"enum\": " + Enum.GetNames(fieldType).ToJSONString<string[]>();
      if (fieldType.IsArray)
        return jsonSchema + ", \"items\": " + AssetImportPipeline.ToJsonSchema(fieldType.GetElementType(), (Colossal.Json.Variant) null).ToJSONString<Colossal.Json.Variant>();
      if (typeof (IList).IsAssignableFrom(fieldType))
        return jsonSchema + ", \"items\": " + AssetImportPipeline.ToJsonSchema(fieldType.GetGenericArguments()[0], (Colossal.Json.Variant) null).ToJSONString<Colossal.Json.Variant>();
      switch (name)
      {
        case "importerTypeHints":
          return jsonSchema + ", \"patternProperties\": { \"^\\\\.[A-Za-z0-9]+$\": { \"$ref\": \"#/definitions/importers\" } }, \"additionalProperties\": false";
        case "sharedAssets":
          return jsonSchema + ", \"patternProperties\": { \"^[A-Za-z0-9_*{}]+(\\\\.[A-Za-z0-9]+)?(/_[A-Za-z0-9]+)?$\": { \"type\": \"string\", \"format\": \"uri-reference\" } }, \"additionalProperties\": false";
        case "importSettings":
          return jsonSchema + ", \"additionalProperties\": { " + AssetImportPipeline.GetSettings() + " }";
        default:
          return jsonSchema;
      }
    }

    private static void AddSimplifiedProperties(Colossal.Json.Variant schema)
    {
      schema["^LOD(\\d+|\\*)?(_[A-Za-z0-9*]+)?$"] = AssetImportPipeline.ToJsonSchema(typeof (LODPostProcessor.PostProcessSettings.LODLevelSettings), (Colossal.Json.Variant) null);
      schema["^Surface(_[A-Za-z0-9*]+)?$"] = AssetImportPipeline.ToJsonSchema(typeof (SurfacePostProcessor.PostProcessSettings), (Colossal.Json.Variant) null);
    }

    public static void GenerateJSONSchema()
    {
      PostProcessorCache.CachePostProcessors();
      ImporterCache.CacheSupportedExtensions();
      Colossal.Json.Variant previous = (Colossal.Json.Variant) new ProxyObject();
      previous["$schema"] = (Colossal.Json.Variant) new ProxyString("http://json-schema.org/draft-07/schema#");
      previous["definitions"] = AssetImportPipeline.GetDefinitions();
      AssetImportPipeline.ToJsonSchema(typeof (Colossal.AssetPipeline.Settings), previous);
      AssetImportPipeline.AddSimplifiedProperties(previous["patternProperties"] = (Colossal.Json.Variant) new ProxyObject());
      previous["additionalProperties"] = (Colossal.Json.Variant) new ProxyBoolean(false);
      string json = previous.ToJSON();
      GUIUtility.systemCopyBuffer = json;
      AssetImportPipeline.log.Info((object) json);
    }

    public static UnityEngine.Material backgroundMaterial
    {
      get
      {
        if ((UnityEngine.Object) AssetImportPipeline.s_BackgroundMaterial == (UnityEngine.Object) null)
        {
          AssetImportPipeline.s_BackgroundMaterial = new UnityEngine.Material(UnityEngine.Shader.Find("HDRP/Unlit"));
          AssetImportPipeline.s_BackgroundMaterial.color = Color.white;
        }
        return AssetImportPipeline.s_BackgroundMaterial;
      }
    }

    private static string AdjustNamingConvention(string input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      for (int index = 0; index < input.Length; ++index)
      {
        char c = input[index];
        if (flag)
        {
          stringBuilder.Append(char.ToUpper(c));
          flag = false;
        }
        else
          stringBuilder.Append(c);
        if (c == '_')
          flag = true;
      }
      return stringBuilder.ToString();
    }

    private static bool IsArtRootPath(
      string rootPathName,
      string path,
      out string artProjectPath,
      out string artProjectRelativePath)
    {
      if (rootPathName == null)
        throw new IOException("rootPath can not be null");
      if (path == null)
        throw new IOException("import path can not be null");
      int num = !(path == rootPathName) ? path.IndexOf(rootPathName) : throw new IOException("rootPath can not be the same as import path");
      bool flag = num != -1;
      artProjectRelativePath = flag ? path.Substring(num + rootPathName.Length + 1).Replace('\\', '/') : (string) null;
      artProjectPath = flag ? path.Substring(0, num + rootPathName.Length).Replace('\\', '/') : (string) null;
      return flag;
    }

    public static bool IsArtRootPath(
      string rootPathName,
      string[] paths,
      out string artProjectPath,
      out List<string> artProjectRelativePaths)
    {
      artProjectPath = (string) null;
      artProjectRelativePaths = new List<string>(paths.Length);
      foreach (string path in paths)
      {
        if (!string.IsNullOrEmpty(path))
        {
          string artProjectPath1;
          string artProjectRelativePath;
          if (!AssetImportPipeline.IsArtRootPath(rootPathName, path, out artProjectPath1, out artProjectRelativePath))
            return false;
          artProjectPath = artProjectPath == null || !(artProjectPath1 != artProjectPath) ? artProjectPath1 : throw new Exception("Root project path does not match. Previous: " + artProjectPath + " Current: " + artProjectPath1);
          artProjectRelativePaths.Add(artProjectRelativePath);
        }
      }
      return true;
    }

    public static IEnumerable<ISettingable> GetImportChainFor(
      Colossal.AssetPipeline.Settings settings,
      SourceAssetCollector.Asset asset,
      ReportBase report)
    {
      List<ISettingable> importChainFor = new List<ISettingable>();
      IAssetImporter importer;
      if (ImporterCache.GetImporter<IAssetImporter>(asset.path, out importer, settings.importerTypeHints))
        importChainFor.Add((ISettingable) importer);
      string assetName;
      try
      {
        AssetUtils.ParseName(Path.GetFileNameWithoutExtension(asset.name), out string _, out assetName, out int _, out int2 _, out Colossal.AssetPipeline.Module _, out int _, out string _, out string _);
      }
      catch (FormatException ex)
      {
        AssetImportPipeline.log.WarnFormat("Invalid filename: {0}", (object) Path.GetFileNameWithoutExtension(asset.name));
        assetName = Path.GetFileName(asset.name);
      }
      if (importer is TextureImporter)
      {
        foreach (ITexturePostProcessor texturePostProcessor in (IEnumerable<ITexturePostProcessor>) PostProcessorCache.GetTexturePostProcessors())
        {
          if (settings.GetPostProcessSettings(asset.name, (ISettingable) texturePostProcessor, report, out ISettings _))
            importChainFor.Add((ISettingable) texturePostProcessor);
        }
      }
      if (importer is ModelImporter)
      {
        foreach (IModelPostProcessor modelPostProcessor in (IEnumerable<IModelPostProcessor>) PostProcessorCache.GetModelPostProcessors())
        {
          if (settings.GetPostProcessSettings(asset.name, (ISettingable) modelPostProcessor, report, out ISettings _))
            importChainFor.Add((ISettingable) modelPostProcessor);
        }
        foreach (IModelSurfacePostProcessor surfacePostProcessor in (IEnumerable<IModelSurfacePostProcessor>) PostProcessorCache.GetModelSurfacePostProcessors())
        {
          if (settings.GetPostProcessSettings(assetName, (ISettingable) surfacePostProcessor, report, out ISettings _))
            importChainFor.Add((ISettingable) surfacePostProcessor);
        }
        foreach (IGeometryPostProcessor geometryPostProcessor in (IEnumerable<IGeometryPostProcessor>) PostProcessorCache.GetGeometryPostProcessors())
        {
          if (settings.GetPostProcessSettings(assetName, (ISettingable) geometryPostProcessor, report, out ISettings _))
            importChainFor.Add((ISettingable) geometryPostProcessor);
        }
      }
      return (IEnumerable<ISettingable>) importChainFor;
    }

    public static IDictionary<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings> CollectDataToImport(
      string projectRootPath,
      string[] assetPaths,
      Report report)
    {
      OrderedDictionary<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings> import = new OrderedDictionary<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings>();
      string artProjectPath;
      List<string> artProjectRelativePaths;
      if (!AssetImportPipeline.IsArtRootPath(projectRootPath, assetPaths, out artProjectPath, out artProjectRelativePaths))
        throw new Exception("Invalid " + artProjectPath);
      using (Report.ImportStep importStep = report.AddImportStep("Collect asset group"))
      {
        foreach (SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset> assetGroup in new SourceAssetCollector(artProjectPath, (IEnumerable<string>) artProjectRelativePaths))
        {
          Report.Asset report1 = report.AddAsset(assetGroup.name);
          Colossal.AssetPipeline.Settings settings = AssetImportPipeline.ImportSettings(assetGroup, (importStep, report1));
          foreach (SourceAssetCollector.Asset file in assetGroup)
          {
            if (settings.ignoreSuffixes != null && Path.GetFileNameWithoutExtension(file.name).EndsWithAny(settings.ignoreSuffixes))
              assetGroup.RemoveFile(file);
          }
          foreach (KeyValuePair<string, string> usedShaderAsset in (IEnumerable<KeyValuePair<string, string>>) settings.UsedShaderAssets(assetGroup, report1))
          {
            string path = AssetImportPipeline.ResolveRelativePath(projectRootPath, usedShaderAsset.Value, assetGroup.rootPath);
            if (LongFile.Exists(path))
            {
              SourceAssetCollector.Asset file = new SourceAssetCollector.Asset(path, projectRootPath);
              assetGroup.AddFile(file);
            }
          }
          import.Add(assetGroup, settings);
        }
      }
      return (IDictionary<SourceAssetCollector.AssetGroup<SourceAssetCollector.Asset>, Colossal.AssetPipeline.Settings>) import;
    }

    private static void CreateTitle(
      Transform parent,
      string text,
      Vector3 textPosOffset,
      Color bgColor,
      Color txtColor,
      int txtSize,
      float txtPadding)
    {
      GameObject gameObject = new GameObject("Title");
      gameObject.transform.SetParent(parent, false);
      gameObject.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
      textMeshPro.font = UnityEngine.Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
      textMeshPro.text = text;
      textMeshPro.fontSize = (float) txtSize;
      textMeshPro.color = txtColor;
      textMeshPro.alignment = TextAlignmentOptions.Center;
      textMeshPro.enableWordWrapping = false;
      Vector2 preferredValues = textMeshPro.GetPreferredValues();
      Vector3 vector3 = new Vector3(preferredValues.x + txtPadding, preferredValues.y + txtPadding, 0.1f);
      gameObject.transform.localPosition = textPosOffset + new Vector3((float) (-(double) preferredValues.x / 2.0), 0.0f, 0.0f);
      AssetImportPipeline.CreateBackground(gameObject.transform, "TextBg", Vector3.zero, new Vector3(vector3.x / 10f, 1f, vector3.y / 10f));
      AssetImportPipeline.backgroundMaterial.color = bgColor;
    }

    private static void CreateBounds(Transform parent)
    {
      Transform parent1 = parent.Find("Title");
      Transform transform = parent1.Find("TextBg");
      AssetImportPipeline.CreateBackground(parent1, "BoundsTop", Vector3.zero, new Vector3(1f, 1f, 0.1f));
      AssetImportPipeline.CreateBackground(parent1, "BoundsBottom", Vector3.zero, new Vector3(1f, 1f, 0.1f));
      AssetImportPipeline.CreateBackground(parent1, "BoundsRight", Vector3.zero, new Vector3(0.1f, 1f, 1f));
      AssetImportPipeline.CreateBackground(parent1, "BoundsLeft", new Vector3((float) (((double) transform.localScale.x * 0.5 + 0.05000000074505806) * 10.0), 0.0f, 0.0f), new Vector3(0.1f, 1f, 1f));
    }

    private static void AdjustZBounds(Transform parent, float z)
    {
      Transform transform1 = parent.Find("Title");
      Transform transform2 = transform1.Find("BoundsTop");
      Transform transform3 = transform1.Find("BoundsBottom");
      Transform transform4 = transform1.Find("BoundsRight");
      Transform transform5 = transform1.Find("BoundsLeft");
      Vector3 localPosition1 = transform2.localPosition with
      {
        y = z
      };
      transform2.localPosition = localPosition1;
      Vector3 localPosition2 = transform3.localPosition with
      {
        y = -z
      };
      transform3.localPosition = localPosition2;
      transform5.localScale = transform5.localScale with
      {
        z = (float) ((double) z * 2.0 / 10.0)
      };
      Vector3 localScale = transform4.localScale with
      {
        z = (float) ((double) z * 2.0 / 10.0)
      };
      transform4.localScale = localScale;
    }

    private static void AdjustXBounds(Transform parent, float x)
    {
      Transform transform1 = parent.Find("Title");
      Transform transform2 = transform1.Find("BoundsTop");
      Transform transform3 = transform1.Find("BoundsBottom");
      Transform transform4 = transform1.Find("BoundsRight");
      Vector3 localPosition1 = transform1.Find("BoundsLeft").transform.localPosition;
      Vector3 vector3 = localPosition1;
      vector3.x += x;
      transform4.transform.localPosition = vector3;
      float num1 = (float) (((double) localPosition1.x + (double) vector3.x) * 0.5);
      Vector3 localPosition2 = transform2.localPosition with
      {
        x = num1
      };
      transform2.localPosition = localPosition2;
      Vector3 localPosition3 = transform3.localPosition with
      {
        x = num1
      };
      transform3.localPosition = localPosition3;
      float num2 = (float) (((double) vector3.x - (double) localPosition1.x) / 10.0 + 0.10000000149011612);
      Vector3 localScale1 = transform2.localScale with
      {
        x = num2
      };
      transform2.localScale = localScale1;
      Vector3 localScale2 = transform3.localScale with
      {
        x = num2
      };
      transform3.localScale = localScale2;
    }

    private static void CreateBackground(
      Transform parent,
      string name,
      Vector3 position,
      Vector3 size)
    {
      GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
      primitive.name = name;
      primitive.transform.SetParent(parent, false);
      primitive.transform.localRotation = Quaternion.Euler(-90f, 0.0f, 0.0f);
      primitive.transform.localScale = size;
      primitive.transform.localPosition = new Vector3(position.x, position.y, position.z + 0.05f);
      primitive.GetComponent<Renderer>().sharedMaterial = AssetImportPipeline.backgroundMaterial;
    }

    public static void InstantiateRenderPrefabs<T>(
      IEnumerable<(T prefab, string sourcePath)> prefabs,
      bool smartInstantiate,
      bool ignoreLODs)
      where T : PrefabBase
    {
      if (smartInstantiate)
      {
        List<(RenderPrefab, string)> list1 = prefabs.Where<(T, string)>((Func<(T, string), bool>) (tuple =>
        {
          if (!((object) tuple.prefab is RenderPrefab))
            return false;
          return !ignoreLODs || !tuple.prefab.name.Contains("_LOD");
        })).Select<(T, string), (RenderPrefab, string)>((Func<(T, string), (RenderPrefab, string)>) (tuple => ((object) tuple.prefab as RenderPrefab, GetParent(tuple.sourcePath)))).OrderBy<(RenderPrefab, string), string>((Func<(RenderPrefab, string), string>) (x => x.Item1.name)).ThenBy<(RenderPrefab, string), string>((Func<(RenderPrefab, string), string>) (x => x.Item2)).ToList<(RenderPrefab, string)>();
        System.Collections.Generic.Dictionary<string, int> dictionary1 = new System.Collections.Generic.Dictionary<string, int>();
        List<float> floatList = new List<float>();
        int num1 = 0;
        foreach ((RenderPrefab, string) tuple in list1)
        {
          Bounds bounds = RenderingUtils.ToBounds(tuple.Item1.bounds);
          int index;
          if (!dictionary1.TryGetValue(tuple.Item2, out index))
          {
            floatList.Add(bounds.extents.z * 1.5f);
            dictionary1.Add(tuple.Item2, num1);
            ++num1;
          }
          else
            floatList[index] = Mathf.Max(floatList[index], bounds.extents.z * 1.5f);
        }
        float element = 5f;
        float num2 = 0.0f;
        System.Collections.Generic.Dictionary<string, GameObject> dictionary2 = new System.Collections.Generic.Dictionary<string, GameObject>(dictionary1.Count);
        List<float> list2 = Enumerable.Repeat<float>(element, dictionary1.Count).ToList<float>();
        foreach ((RenderPrefab, string) tuple in list1)
        {
          int index = dictionary1[tuple.Item2];
          GameObject gameObject1;
          if (!dictionary2.TryGetValue(tuple.Item2, out gameObject1))
          {
            string fileName = Path.GetFileName(tuple.Item2);
            gameObject1 = GameObject.Find(fileName);
            if ((UnityEngine.Object) gameObject1 == (UnityEngine.Object) null)
            {
              gameObject1 = new GameObject(fileName);
              AssetImportPipeline.CreateTitle(gameObject1.transform, fileName, new Vector3(0.0f, 0.1f, 0.0f), (Color) new Color32((byte) 1, (byte) 174, (byte) 240, byte.MaxValue), Color.white, 48, 0.1f);
              AssetImportPipeline.CreateBounds(gameObject1.transform);
            }
            float z = num2 + floatList[index];
            gameObject1.transform.position = new Vector3(0.0f, 0.0f, z);
            num2 = z + (floatList[index] + 10f);
            AssetImportPipeline.AdjustZBounds(gameObject1.transform, floatList[index] + 5f);
            dictionary2.Add(tuple.Item2, gameObject1);
          }
          if ((UnityEngine.Object) GameObject.Find(gameObject1.name + "/" + tuple.Item1.name) == (UnityEngine.Object) null)
          {
            GameObject gameObject2 = new GameObject(tuple.Item1.name);
            gameObject2.transform.parent = gameObject1.transform;
            gameObject2.AddComponent<RenderPrefabRenderer>().m_Prefab = tuple.Item1;
            Bounds bounds = RenderingUtils.ToBounds(tuple.Item1.bounds);
            list2[index] += bounds.extents.x * 1.5f;
            gameObject2.transform.localPosition = new Vector3(list2[index], 0.0f, 0.0f);
            list2[index] += bounds.extents.x * 1.5f + element;
            AssetImportPipeline.AdjustXBounds(gameObject1.transform, list2[index]);
          }
        }
      }
      else
      {
        int num3 = 0;
        float a = 0.0f;
        float num4 = 0.0f;
        float z = 0.0f;
        foreach ((T prefab, string sourcePath) prefab1 in prefabs)
        {
          if ((!ignoreLODs || !prefab1.prefab.name.Contains("_LOD")) && prefab1.prefab is RenderPrefab prefab2 && (UnityEngine.Object) GameObject.Find(prefab2.name) == (UnityEngine.Object) null)
          {
            GameObject gameObject = new GameObject(prefab2.name);
            gameObject.AddComponent<RenderPrefabRenderer>().m_Prefab = prefab2;
            Bounds bounds = RenderingUtils.ToBounds(prefab2.bounds);
            float x = num4 + bounds.extents.x * 1.5f;
            gameObject.transform.position = new Vector3(x, 0.0f, z);
            num4 = x + bounds.extents.x * 1.5f;
            a = Mathf.Max(a, bounds.extents.z * 3f);
            ++num3;
            if (num3 % 10 == 0)
            {
              z += a;
              a = 0.0f;
              num4 = 0.0f;
            }
          }
        }
      }

      static string GetParent(string path)
      {
        int length = path.LastIndexOf('/');
        return length < 0 ? path : path.Substring(0, length);
      }
    }

    public static System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> GetTextureReferenceCount(
      IEnumerable<SurfaceAsset> surfaces,
      out int surfaceCount)
    {
      System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> textureReferenceCount = new System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>>();
      surfaceCount = 0;
      foreach (SurfaceAsset surface in surfaces)
      {
        using (surface)
        {
          surface.LoadProperties(false);
          foreach (KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset> texture in (IEnumerable<KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset>>) surface.textures)
          {
            List<SurfaceAsset> surfaceAssetList;
            if (!textureReferenceCount.TryGetValue(texture.Value, out surfaceAssetList))
            {
              surfaceAssetList = new List<SurfaceAsset>();
              textureReferenceCount.Add(texture.Value, surfaceAssetList);
            }
            surfaceAssetList.Add(surface);
          }
          ++surfaceCount;
        }
      }
      return textureReferenceCount;
    }

    private static void ReportTextureReferenceStats(
      System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> textureReferencesMap,
      Report.ImportStep report)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (Colossal.IO.AssetDatabase.TextureAsset key in textureReferencesMap.Keys)
      {
        if (textureReferencesMap[key].Count == 1)
          ++num1;
        else if (textureReferencesMap[key].Count == 2)
          ++num2;
        else
          ++num3;
      }
      report.AddMessage(string.Format("Singles: {0}", (object) num1));
      report.AddMessage(string.Format("Doubles: {0}", (object) num2));
      report.AddMessage(string.Format("Multiple: {0}", (object) num3));
    }

    private static int TestTextureSizesUniformity(
      SurfaceAsset asset,
      int tileSize,
      MaterialLibrary.MaterialDescription description)
    {
      int length = description.m_Stacks.Length;
      int[] numArray1 = new int[length];
      int[] numArray2 = new int[length];
      for (int index = 0; index < length; ++index)
        numArray1[index] = -1;
      foreach (KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset> texture in (IEnumerable<KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset>>) asset.textures)
      {
        int stackConfigIndex = description.GetStackConfigIndex(texture.Key);
        if (stackConfigIndex != -1)
        {
          Colossal.IO.AssetDatabase.TextureAsset textureAsset = texture.Value;
          textureAsset.LoadData(0);
          if (textureAsset.width < tileSize)
            return -1;
          if (textureAsset.height < tileSize)
            return -2;
          if (numArray1[stackConfigIndex] == -1)
          {
            numArray1[stackConfigIndex] = textureAsset.width;
            numArray2[stackConfigIndex] = textureAsset.height;
          }
          else if (numArray1[stackConfigIndex] != textureAsset.width || numArray2[stackConfigIndex] != textureAsset.height)
            return -4;
        }
      }
      return 0;
    }

    public static void ProcessSurfacesForVT(
      IEnumerable<SurfaceAsset> surfacesToConvert,
      IEnumerable<SurfaceAsset> surfaces,
      bool force,
      Report.ImportStep report)
    {
      int midMipsCount = 3;
      int tileSize = 512;
      int mipBias = 20;
      AssetImportPipeline.ConvertSurfacesToVT(surfacesToConvert, surfaces, false, tileSize, midMipsCount, mipBias, force, report);
      AssetImportPipeline.BuildMidMipsCache(surfaces, tileSize, midMipsCount, Colossal.IO.AssetDatabase.AssetDatabase.game);
      AssetImportPipeline.HideVTSourceTextures(surfacesToConvert);
      AssetImportPipeline.ResaveCache(report);
    }

    public static void ConvertSurfacesToVT(
      IEnumerable<SurfaceAsset> surfacesToConvert,
      IEnumerable<SurfaceAsset> allSurfaces,
      bool force,
      Report.ImportStep report)
    {
      int midMipsCount = 3;
      int tileSize = 512;
      int mipBias = 20;
      AssetImportPipeline.ConvertSurfacesToVT(surfacesToConvert, allSurfaces, false, tileSize, midMipsCount, mipBias, force, report);
    }

    public static void ConvertSurfacesToVT(
      IEnumerable<SurfaceAsset> surfacesToConvert,
      IEnumerable<SurfaceAsset> allSurfaces,
      bool writeVTSettings,
      int tileSize,
      int midMipsCount,
      int mipBias,
      bool force,
      Report.ImportStep report)
    {
      AssetImportPipeline.s_Progress.Set("VT post process - Converting surfaces", "Collecting references...", 0.0f);
      int surfaceCount;
      System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> textureReferenceCount = AssetImportPipeline.GetTextureReferenceCount(allSurfaces, out surfaceCount);
      AssetImportPipeline.ReportTextureReferenceStats(textureReferenceCount, report);
      MaterialLibrary materialLibrary = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.materialLibrary;
      VirtualTexturingConfig virtualTexturingConfig = UnityEngine.Resources.Load<VirtualTexturingConfig>("VirtualTexturingConfig");
      int num = 0;
      foreach (SurfaceAsset surfaceAsset in surfacesToConvert)
      {
        AssetImportPipeline.s_Progress.Set("VT post process - Converting surfaces", "Processing " + surfaceAsset.name, (float) num++ / (float) surfaceCount);
        try
        {
          bool flag = surfaceAsset.IsVTMaterialFromHeader();
          if (!force)
          {
            if (flag)
              continue;
          }
          surfaceAsset.LoadProperties(false);
          MaterialLibrary.MaterialDescription materialDescription = materialLibrary.GetMaterialDescription(surfaceAsset.materialTemplateHash);
          if (materialDescription != null)
          {
            if (materialDescription.m_SupportsVT)
            {
              switch (AssetImportPipeline.TestTextureSizesUniformity(surfaceAsset, tileSize, materialDescription))
              {
                case -5:
                  AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because some of its textures are null", (object) surfaceAsset);
                  break;
                case -4:
                  AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because its texture sizes is not uniform", (object) surfaceAsset);
                  break;
                case -3:
                  AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because at least one texture uses a wrap mode that is not Clamp", (object) surfaceAsset);
                  break;
                case -2:
                  AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because at least one of its textures height is smaller than the tileSize {1}", (object) surfaceAsset, (object) tileSize);
                  break;
                case -1:
                  AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because at least one of its textures width is smaller than the tileSize {1}", (object) surfaceAsset, (object) tileSize);
                  break;
                case 0:
                  if (materialDescription.m_SupportsVT)
                  {
                    int mipBias1 = materialDescription.hasMipBiasOverride ? materialDescription.m_MipBiasOverride : mipBias;
                    if (surfaceAsset.Save(mipBias1, true, vt: true, virtualTexturingConfig: virtualTexturingConfig, textureReferencesMap: textureReferenceCount, tileSize: new int?(tileSize), nbMidMipLevelsRequested: new int?(midMipsCount)))
                    {
                      AssetImportPipeline.log.InfoFormat("File {0} has been converted to VT", (object) surfaceAsset);
                      continue;
                    }
                    continue;
                  }
                  break;
              }
            }
            else
              AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because its template {2} (Shader:{3}) from material hash {1} is not set to support VT", (object) surfaceAsset, (object) surfaceAsset.materialTemplateHash, (object) materialDescription.m_Material.name, (object) materialDescription.m_Material.shader.name);
          }
          else
            AssetImportPipeline.log.WarnFormat("File {0} cannot use VT because its material hash {1} is not mapped or not found", (object) surfaceAsset, (object) surfaceAsset.materialTemplateHash);
          if (flag)
          {
            surfaceAsset.Save(force: true, saveTextures: false);
            AssetImportPipeline.log.InfoFormat("File {0} has been unconverted from VT", (object) surfaceAsset);
          }
        }
        catch (Exception ex)
        {
          AssetImportPipeline.log.ErrorFormat(ex, "Error occured with {0}", (object) surfaceAsset);
          throw;
        }
        finally
        {
          surfaceAsset.Unload(false);
        }
      }
      if (!writeVTSettings)
        return;
      using (VTSettingsAsset vtSettingsAsset = Colossal.IO.AssetDatabase.AssetDatabase.game.AddAsset<VTSettingsAsset>(AssetDataPath.Create(EnvPath.kVTSubPath, "VT")))
        vtSettingsAsset.Save(mipBias, tileSize, midMipsCount);
    }

    private static void ResaveCache(Report.ImportStep report)
    {
      AssetImportPipeline.s_Progress.Set("VT post process", "Resaving asset cache", 100f);
      report.AddMessage(Colossal.IO.AssetDatabase.AssetDatabase.global.ResaveCache().Result);
    }

    public static void HideVTSourceTextures(IEnumerable<SurfaceAsset> surfaces)
    {
      AssetImportPipeline.s_Progress.Set("VT post process - Hiding converted textures", "", 0.0f);
      System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> references1 = new System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>>();
      System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> references2 = new System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>>();
      foreach (SurfaceAsset surface in surfaces)
      {
        surface.LoadProperties(true);
        if (surface.isVTMaterial)
        {
          foreach (KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset> texture in (IEnumerable<KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset>>) surface.textures)
          {
            if (surface.IsHandledByVirtualTexturing(texture))
              AddReferenceTo(references1, texture.Value, surface);
            else
              AddReferenceTo(references2, texture.Value, surface);
          }
        }
        else
        {
          foreach (KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset> texture in (IEnumerable<KeyValuePair<string, Colossal.IO.AssetDatabase.TextureAsset>>) surface.textures)
            AddReferenceTo(references2, texture.Value, surface);
        }
        surface.Unload(false);
      }
      List<Colossal.IO.AssetDatabase.TextureAsset> list = Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<Colossal.IO.AssetDatabase.TextureAsset>(new SearchFilter<Colossal.IO.AssetDatabase.TextureAsset>()).ToList<Colossal.IO.AssetDatabase.TextureAsset>();
      for (int index = 0; index < list.Count; ++index)
      {
        Colossal.IO.AssetDatabase.TextureAsset textureAsset = list[index];
        AssetImportPipeline.s_Progress.Set("VT post process - Hiding", "Processing " + textureAsset.name, (float) index / (float) references1.Count);
        if (references1.ContainsKey(textureAsset))
        {
          if (references2.ContainsKey(textureAsset))
          {
            AssetImportPipeline.log.WarnFormat("Texture {0} is referenced {1} times by VT materials and {2} times by non VT materials. It will be duplicated on disk.", (object) textureAsset, (object) references1[textureAsset].Count, (object) references2[textureAsset].Count);
            AssetImportPipeline.log.InfoFormat("Detail for {0}:\nvt: {1}\nnon vt: {2}", (object) textureAsset, (object) string.Join<SurfaceAsset>(", ", (IEnumerable<SurfaceAsset>) references1[textureAsset]), (object) string.Join<SurfaceAsset>(", ", (IEnumerable<SurfaceAsset>) references2[textureAsset]));
          }
          else
            AssetImportPipeline.log.InfoFormat(string.Format("Hiding {0}", (object) textureAsset));
        }
      }

      static void AddReferenceTo(
        System.Collections.Generic.Dictionary<Colossal.IO.AssetDatabase.TextureAsset, List<SurfaceAsset>> references,
        Colossal.IO.AssetDatabase.TextureAsset texture,
        SurfaceAsset surface)
      {
        List<SurfaceAsset> surfaceAssetList;
        if (!references.TryGetValue(texture, out surfaceAssetList))
        {
          surfaceAssetList = new List<SurfaceAsset>();
          references.Add(texture, surfaceAssetList);
        }
        surfaceAssetList.Add(surface);
      }
    }

    public static void BuildMidMipsCache(
      IEnumerable<SurfaceAsset> surfaces,
      int tileSize,
      int midMipsCount,
      ILocalAssetDatabase database)
    {
      AssetImportPipeline.s_Progress.Set("VT post process - Rebuilding mip cache", "", 0.0f);
      if (midMipsCount < 0)
        throw new Exception("Nb mid mip levels can't be negative");
      VirtualTexturingConfig virtualTexturingConfig = UnityEngine.Resources.Load<VirtualTexturingConfig>("VirtualTexturingConfig");
      int length = virtualTexturingConfig.stackDatas.Length;
      MaterialLibrary materialLibrary = Colossal.IO.AssetDatabase.AssetDatabase.global.resources.materialLibrary;
      int tileSize1 = tileSize;
      int midMipLevelsCount = midMipsCount;
      AtlasMaterialsGrouper materialsGrouper = new AtlasMaterialsGrouper(length, tileSize1, midMipLevelsCount);
      List<SurfaceAsset> list = surfaces.ToList<SurfaceAsset>();
      System.Collections.Generic.Dictionary<Colossal.Hash128, NativeArray<byte>> dictionary = new System.Collections.Generic.Dictionary<Colossal.Hash128, NativeArray<byte>>();
      System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.Hash128[]>[] materialTextures = new System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.Hash128[]>[2];
      for (int index = 0; index < materialTextures.Length; ++index)
        materialTextures[index] = new System.Collections.Generic.Dictionary<Colossal.Hash128, Colossal.Hash128[]>();
      for (int index = 0; index < list.Count; ++index)
      {
        using (SurfaceAsset surface = list[index])
        {
          AssetImportPipeline.s_Progress.Set("VT post process - Rebuilding mip cache", "Processing " + surface.name, (float) index / (float) list.Count);
          surface.LoadProperties(true);
          if (surface.isVTMaterial)
          {
            MaterialLibrary.MaterialDescription materialDescription = materialLibrary.GetMaterialDescription(surface.materialTemplateHash);
            long[] textureHash = new long[materialDescription.m_Stacks.Length];
            for (int stackConfigIndex = 0; stackConfigIndex < materialDescription.m_Stacks.Length; ++stackConfigIndex)
            {
              Colossal.Hash128[] hash128Array = new Colossal.Hash128[4];
              materialTextures[stackConfigIndex][surface.guid] = hash128Array;
              surface.AddMidMipTexturesDataToDictionnary(stackConfigIndex, midMipsCount, tileSize, materialDescription, dictionary);
            }
            int vtLayersMask = surface.ComputeVTLayersMask(materialDescription, materialTextures, textureHash);
            for (int stackConfigIndex = 0; stackConfigIndex < surface.stackCount; ++stackConfigIndex)
            {
              AtlassedSize stackTextureSize = surface.GetUnbiasedStackTextureSize(stackConfigIndex);
              if (stackTextureSize.x >= 0)
                materialsGrouper.Add(stackConfigIndex, stackTextureSize, vtLayersMask, surface, textureHash, materialDescription.m_MipBiasOverride);
            }
          }
        }
      }
      materialsGrouper.ResolveDuplicates(materialTextures, 2);
      materialsGrouper.GroupEntries(virtualTexturingConfig, dictionary, materialTextures);
      string assetName = AtlasMaterialsGrouper.GetAssetName(tileSize, midMipsCount);
      using (BinaryWriter bw = new BinaryWriter(database.AddAsset<MidMipCacheAsset>(AssetDataPath.Create("StreamingData~", assetName)).GetWriteStream()))
        materialsGrouper.Write(bw);
      foreach (NativeArray<byte> nativeArray in dictionary.Values)
        nativeArray.Dispose();
      materialsGrouper.Dispose();
    }

    public static async System.Threading.Tasks.Task ApplyVTMipBias(
      IAssetDatabase database,
      int mipBias,
      int tileSize,
      int midMipCount,
      string folder)
    {
      if (AssetImportPipeline.s_MainThreadDispatcher == null)
        AssetImportPipeline.s_MainThreadDispatcher = new MainThreadDispatcher();
      if (mipBias < 0)
        throw new Exception("Mip bias cannot be smaller than zero in that context!");
      bool flag = true;
      string str = (string) null;
      if (database is ILocalAssetDatabase localAssetDatabase && localAssetDatabase.dataSource is FileSystemDataSource dataSource)
      {
        str = dataSource.rootPath + "/StreamingData~";
        if (database != Colossal.IO.AssetDatabase.AssetDatabase.game)
          flag = false;
      }
      ILocalAssetDatabase vtMipXDatabase = str != null ? Colossal.IO.AssetDatabase.AssetDatabase.GetTransient(rootPath: str + "/." + folder) : throw new ArgumentException("Master VT file path is null.");
      VirtualTexturingConfig virtualTexturingConfig = UnityEngine.Resources.Load<VirtualTexturingConfig>("VirtualTexturingConfig");
      ParallelOptions opts = new ParallelOptions()
      {
        MaxDegreeOfParallelism = AssetImportPipeline.useParallelImport ? System.Environment.ProcessorCount : 1
      };
      int total = 0;
      System.Threading.Tasks.Task importTask = System.Threading.Tasks.Task.Run((System.Action) (() =>
      {
        List<VTTextureAsset> texture2DPreProcessedAssets = database.GetAssets<VTTextureAsset>().ToList<VTTextureAsset>();
        List<SurfaceAsset> list = database.GetAssets<SurfaceAsset>().ToList<SurfaceAsset>();
        int assetsToProcess = texture2DPreProcessedAssets.Count + list.Count;
        for (int index = 0; index < list.Count; ++index)
        {
          try
          {
            using (SurfaceAsset p1 = list[index])
            {
              AssetImportPipeline.s_Progress.Set("VT post process - Apply Surface MipBias", "Applying Mip Bias to " + p1.name, (float) total / (float) assetsToProcess);
              AssetImportPipeline.log.InfoFormat("Processing {0} ({1}/{2})", (object) p1, (object) (total + 1), (object) assetsToProcess);
              if (!AssetImportPipeline.s_Progress.shouldCancel)
              {
                p1.LoadProperties(false);
                if (p1.isVTMaterial)
                {
                  if (p1.hasVTSurfaceAsset)
                    p1.UpdateMipBias(vtMipXDatabase, mipBias, virtualTexturingConfig, tileSize, midMipCount);
                }
              }
              else
                break;
            }
            Interlocked.Increment(ref total);
          }
          catch (Exception ex)
          {
            AssetImportPipeline.log.ErrorFormat(ex, "Error with {0}", (object) list[index]);
          }
        }
        Parallel.ForEach<VTTextureAsset>((IEnumerable<VTTextureAsset>) texture2DPreProcessedAssets, opts, (Action<VTTextureAsset, ParallelLoopState, long>) ((texture2DPreProcessedAsset, state, index) =>
        {
          try
          {
            AssetImportPipeline.s_Progress.Set("VT post process - Apply Texture MipBias", "Applying Mip Bias to " + texture2DPreProcessedAsset.name, (float) total / (float) assetsToProcess);
            AssetImportPipeline.log.InfoFormat("Processing {0} ({1}/{2})", (object) texture2DPreProcessedAsset, (object) (total + 1), (object) assetsToProcess);
            if (AssetImportPipeline.s_Progress.shouldCancel)
              state.Stop();
            texture2DPreProcessedAsset.LoadHeader();
            using (Colossal.IO.AssetDatabase.TextureAsset textureAsset = texture2DPreProcessedAsset.textureAsset)
            {
              textureAsset.LoadData(0);
              if (textureAsset.width < tileSize || textureAsset.height < tileSize)
                AssetImportPipeline.log.ErrorFormat("That texture [{0}] dimension is too small to be supported by the VT system textureSize: {1}x{2} VT tileSize: {3}", (object) textureAsset.name, (object) textureAsset.width, (object) textureAsset.height, (object) tileSize);
              using (VTTextureAsset vtTextureAsset = vtMipXDatabase.AddAsset<VTTextureAsset>((AssetDataPath) texture2DPreProcessedAsset.name, texture2DPreProcessedAsset.guid))
                vtTextureAsset.Save(mipBias, textureAsset, tileSize, midMipCount, virtualTexturingConfig);
            }
            Interlocked.Increment(ref total);
          }
          catch (Exception ex)
          {
            AssetImportPipeline.log.ErrorFormat(ex, "Error with {0}", (object) texture2DPreProcessedAssets);
          }
        }));
      }));
      if (flag)
      {
        using (VTSettingsAsset vtSettingsAsset = vtMipXDatabase.AddAsset<VTSettingsAsset>((AssetDataPath) "VT"))
          vtSettingsAsset.Save(mipBias, tileSize, midMipCount);
      }
      Report report = new Report();
      await AssetImportPipeline.ExecuteMainThreadQueue(importTask, report);
    }

    private class ThemesConfig
    {
      public string[] themePrefixes;
    }

    private class Progress
    {
      private string title;
      private string description;
      private string threadDescription;
      private float value;
      private Func<string, string, float, bool> progressCallback;
      private Thread mainThread;

      public bool shouldCancel { get; private set; }

      public Progress() => this.mainThread = Thread.CurrentThread;

      public void SetHandler(Func<string, string, float, bool> progressCallback)
      {
        this.progressCallback = progressCallback;
      }

      public void Reset(Func<string, string, float, bool> progressCallback)
      {
        this.progressCallback = progressCallback;
        this.shouldCancel = false;
      }

      public void Set(string title, string description, float value)
      {
        this.title = title;
        this.description = description;
        double num = (double) Interlocked.Exchange(ref this.value, value);
        if (this.mainThread != Thread.CurrentThread)
          return;
        this.Update();
      }

      public AssetImportPipeline.Progress.ScopedThreadDescriptionObject ScopedThreadDescription(
        string description)
      {
        return new AssetImportPipeline.Progress.ScopedThreadDescriptionObject(this, description);
      }

      public void SetThreadDescription(string description)
      {
        this.threadDescription = description;
        if (this.mainThread != Thread.CurrentThread)
          return;
        this.Update();
      }

      public void Set(string title, string description)
      {
        this.title = title;
        this.description = description;
        if (this.mainThread != Thread.CurrentThread)
          return;
        this.Update();
      }

      public void Update()
      {
        string str = this.threadDescription ?? this.description;
        if (this.shouldCancel)
          str += " (Canceled)";
        if (this.progressCallback == null || !this.progressCallback(this.title, str, this.value))
          return;
        this.shouldCancel = true;
      }

      public class ScopedThreadDescriptionObject : IDisposable
      {
        private AssetImportPipeline.Progress owner;

        public ScopedThreadDescriptionObject(AssetImportPipeline.Progress owner, string description)
        {
          this.owner = owner;
          owner.SetThreadDescription(description);
        }

        public void Dispose() => this.owner.SetThreadDescription((string) null);
      }
    }
  }
}
