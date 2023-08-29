/*    
 *   Copyright (c) 2023 amazeedaizee
 *   
 *   Permission is hereby granted, free of charge, to any person obtaining a copy of this software (CustomAssetExtractor.cs) and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *   
 *   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *   
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 *   ---------
 *   
 *   Parts of the code in this file uses: 
 *   the AssetTools.NET Library, 
 *   the AssetRipper.TextureDecoder Library,
 *   and the ImageSharp library (Apache 2.0 version since this software is visible-source).
 *
 *   Licenses:
 *   
 *   https://github.com/nesrak1/AssetsTools.NET/blob/master/LICENSE
 *   https://github.com/AssetRipper/TextureDecoder/blob/master/LICENSE
 *   https://github.com/SixLabors/ImageSharp/blob/main/LICENSE
*/

using AssetsTools.NET.Extra;
using AssetsTools.NET.Texture;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    public enum CustomAssetType
    {
        Background, Sprite, EndScreen
    }

    public enum CustomAssetFileType
    {
        ImageFile, AssetBundle, AddressableBundle
    }

    [Serializable]
    public class CustomAsset
    {
        public CustomAssetType customAssetType;
        public CustomAssetFileType customAssetFileType;
        public string fileName;
        public string filePath;
        public string catalogPath;
        public int picWidth;
        public int picHeight;

        public CustomAsset() { }
        public CustomAsset(CustomAssetType customAssetType, CustomAssetFileType customAssetFileType, string filename, string filePath)
        {
            this.customAssetType = customAssetType;
            this.customAssetFileType = customAssetFileType;
            this.fileName = filename;
            this.filePath = filePath;
            if (customAssetFileType == CustomAssetFileType.AddressableBundle)
            {
                catalogPath = CustomAssetExtractor.catalogPath;
            }
        }

        public static bool IsCustomAssetTheSame(CustomAsset asset1, CustomAsset asset2)
        {
            if (asset1.customAssetType != asset2.customAssetType)
                return false;
            if (asset1.customAssetFileType != asset2.customAssetFileType)
                return false;
            if (asset1.fileName != asset2.fileName)
                return false;
            if (asset1.filePath != asset2.filePath)
                return false;
            if (asset1.catalogPath != asset2.catalogPath)
                return false;
            if (asset1.picWidth != asset2.picWidth)
                return false;
            if (asset1.picHeight != asset2.picHeight)
                return false;
            return true;
        }
    }

    public class CustomAssetSettings
    {
        public List<CustomAsset> customAssets;
        public string catalogPath;

        public CustomAssetSettings() { }
    }
    internal class CustomAssetExtractor
    {
        internal const string VALID_CATALOG_STRING = @"""m_InstanceProviderData"":{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.InstanceProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.InstanceProvider""},""m_Data"":""""},""m_SceneProviderData"":{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.SceneProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.SceneProvider""},""m_Data"":""""},""m_ResourceProviderData"":[{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.LegacyResourcesProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.LegacyResourcesProvider""},""m_Data"":""""},{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.AssetBundleProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.AssetBundleProvider""},""m_Data"":""""},{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.BundledAssetProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.BundledAssetProvider""},""m_Data"":""""},{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.LegacyResourcesProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.LegacyResourcesProvider""},""m_Data"":""""},{""m_Id"":""UnityEngine.ResourceManagement.ResourceProviders.BundledAssetProvider"",""m_ObjectType"":{""m_AssemblyName"":""Unity.ResourceManager, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"",""m_ClassName"":""UnityEngine.ResourceManagement.ResourceProviders.BundledAssetProvider""},""m_Data"":""""}";

        internal static string cachedAnimPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
        internal static List<CustomAsset> customAssets = new();
        internal static string catalogPath = null;

        internal static void CheckForMissingFilesInSettings(ref StreamSettings settings)
        {
            var currentAssets = new List<CustomAsset>();
            var missingAssets = new List<CustomAsset>();
            if (settings.CustomBackground != null)
                currentAssets.Add(settings.CustomBackground);
            if (settings.CustomStartingAnimation != null)
                currentAssets.Add(settings.CustomStartingAnimation);
            GetUniqueAssetsInSettings(ref settings);
            if (currentAssets.Count == 0)
                return;
            CacheValidAssets();
            if (missingAssets.Count == 0)
                return;
            PromptMissingFiles(missingAssets, ref currentAssets);
            ReplaceMissingAssetsWithDefault(ref settings);

            void GetUniqueAssetsInSettings(ref StreamSettings settings)
            {
                foreach (var playObj in settings.PlayingList)
                {
                    if (playObj is KAngelSays)
                    {
                        var kObj = playObj as KAngelSays;
                        if (kObj.customAnim != null && (currentAssets.Count == 0 || !currentAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(kObj.customAnim, a))))
                            currentAssets.Add(kObj.customAnim);
                        continue;
                    }
                    if (playObj is ChatSays)
                    {
                        var chatObj = playObj as ChatSays;
                        if (chatObj.Replies != null)
                        {
                            foreach (var reply in chatObj.Replies)
                            {
                                if (reply.customAnim != null && (currentAssets.Count == 0 || !currentAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(reply.customAnim, a))))
                                    currentAssets.Add(reply.customAnim);
                            }
                        }
                    }
                }
            }

            void CacheValidAssets()
            {
                var assetsToSync = new List<CustomAsset>();
                var pathsToSearch = new List<(string bundle, bool isAddressable)>();
                foreach (var asset in currentAssets)
                {
                    var sameAssetDifferentPath = customAssets.Find(a => a.fileName == asset.fileName && a.customAssetFileType == asset.customAssetFileType && a.filePath != asset.filePath);
                    if (sameAssetDifferentPath != null && File.Exists(asset.filePath) && !File.Exists(sameAssetDifferentPath.filePath))
                    {
                        sameAssetDifferentPath.filePath = asset.filePath;
                        continue;
                    }
                    if (sameAssetDifferentPath != null && !File.Exists(asset.filePath) && File.Exists(sameAssetDifferentPath.filePath))
                    {
                        asset.filePath = sameAssetDifferentPath.filePath;
                        continue;
                    }
                    if (!customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, asset)) && File.Exists(asset.filePath))
                    {
                        AddValidAssetToCusAssets(asset, ref assetsToSync, ref pathsToSearch);
                        continue;
                    }
                    if (!File.Exists(asset.filePath) && !missingAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(asset, a)))
                        missingAssets.Add(asset);
                }
                SaveCustomAssets();
                foreach (var path in pathsToSearch)
                {
                    List<string> clipNames = new();
                    var selectedAssets = assetsToSync.FindAll(a => a.filePath == path.bundle && a.customAssetType == CustomAssetType.Sprite);
                    foreach (var asset in selectedAssets)
                    {
                        clipNames.Add(asset.fileName);
                    }
                    ImportSelectedSpritesFromAssetBundle(path.bundle, clipNames, path.isAddressable);
                }
            }

            void ReplaceMissingAssetsWithDefault(ref StreamSettings settings)
            {
                foreach (var playObj in settings.PlayingList)
                {
                    if (playObj is KAngelSays)
                    {
                        var kObj = playObj as KAngelSays;
                        if (kObj.customAnim != null && currentAssets.Exists(a => a.customAssetType == kObj.customAnim.customAssetType && a.fileName == kObj.AnimName))
                        {
                            var assetToCompare = currentAssets.Find(a => a.customAssetType == kObj.customAnim.customAssetType && a.fileName == kObj.AnimName);
                            if (kObj.customAnim.filePath != assetToCompare.filePath)
                            {
                                kObj.customAnim.customAssetFileType = assetToCompare.customAssetFileType;
                                kObj.customAnim.catalogPath = assetToCompare.catalogPath;
                                kObj.customAnim.filePath = assetToCompare.filePath;
                            }
                        }
                        continue;
                    }
                    if (playObj is ChatSays)
                    {
                        var chatObj = playObj as ChatSays;
                        if (chatObj.Replies != null)
                        {
                            foreach (var reply in chatObj.Replies)
                            {
                                if (reply.customAnim != null && currentAssets.Exists(a => a.customAssetType == reply.customAnim.customAssetType && a.fileName == reply.AnimName))
                                {
                                    var assetToCompare = currentAssets.Find(a => a.customAssetType == reply.customAnim.customAssetType && a.fileName == reply.AnimName);
                                    if (reply.customAnim.filePath != assetToCompare.filePath)
                                    {
                                        reply.customAnim.customAssetFileType = assetToCompare.customAssetFileType;
                                        reply.customAnim.catalogPath = assetToCompare.catalogPath;
                                        reply.customAnim.filePath = assetToCompare.filePath;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void AddValidAssetToCusAssets(CustomAsset asset, ref List<CustomAsset> assetToAdd, ref List<(string, bool)> pathToSearch)
        {
            if (asset == null) return;
            if (!string.IsNullOrEmpty(asset.filePath) && File.Exists(asset.filePath) && !customAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(a, asset)))
            {
                CustomAsset newAsset = new CustomAsset(asset.customAssetType, asset.customAssetFileType, asset.fileName, asset.filePath);
                newAsset.catalogPath = asset.catalogPath;
                newAsset.picWidth = asset.picWidth;
                newAsset.picHeight = asset.picHeight;
                if (asset.customAssetType == CustomAssetType.Sprite)
                    assetToAdd.Add(newAsset);
                else customAssets.Add(newAsset);
            }
            if (!pathToSearch.Exists(p => p.Item1 == asset.filePath) && asset.customAssetType == CustomAssetType.Sprite)
                pathToSearch.Add(new(asset.filePath, asset.customAssetFileType == CustomAssetFileType.AddressableBundle));
        }
        internal static void PromptMissingFiles(List<CustomAsset> missingFiles, ref List<CustomAsset> assetsToEdit)
        {
            bool ignoreAll = false;
            var bundlesToLoad = new List<(string bundle, bool isAddressable)>();
            var clipsToLoad = new List<(string clipName, string bundle)>();
            foreach (var asset in missingFiles)
            {
                var editedAsset = assetsToEdit.Find(a => a == asset);
                if (!File.Exists(asset.filePath))
                {
                    if (ignoreAll)
                    {
                        if (!editedAsset.filePath.Contains("?"))
                            editedAsset.filePath += "?";
                        continue;
                    }
                    MissingFileMessage missingFileMessage = new(asset);
                    missingFileMessage.ShowDialog();
                    var type = missingFileMessage.missing.customAssetFileType;
                    editedAsset.customAssetFileType = type;
                    editedAsset.catalogPath = missingFileMessage.missing.catalogPath;
                    if (missingFileMessage.isIgnoreAll)
                    {
                        ignoreAll = true;
                    }
                    if (string.IsNullOrEmpty(missingFileMessage.newPath))
                    {
                        if (!editedAsset.filePath.Contains("?"))
                            editedAsset.filePath += "?";
                    }
                    else if (type == CustomAssetFileType.ImageFile)
                    {
                        editedAsset.filePath = missingFileMessage.newPath;
                        if (!customAssets.Exists(a => a.fileName == editedAsset.fileName))
                        {
                            CustomAsset newAsset = new CustomAsset(asset.customAssetType, asset.customAssetFileType, asset.fileName, asset.filePath);
                            newAsset.catalogPath = asset.catalogPath;
                            newAsset.picWidth = asset.picWidth;
                            newAsset.picHeight = asset.picHeight;
                            customAssets.Add(newAsset);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(missingFileMessage.newPath) && (!bundlesToLoad.Exists(b => b.bundle == missingFileMessage.newPath)))
                        {
                            bundlesToLoad.Add(new(missingFileMessage.newPath, type == CustomAssetFileType.AddressableBundle));
                        }
                        if (!string.IsNullOrEmpty(missingFileMessage.newPath) && (!clipsToLoad.Contains((missingFileMessage.missing.fileName, missingFileMessage.newPath))))
                        {
                            clipsToLoad.Add(new(missingFileMessage.missing.fileName, missingFileMessage.newPath));
                        }
                        editedAsset.filePath = missingFileMessage.newPath;
                    }
                    missingFileMessage.Dispose();
                }
            }
            foreach (var path in bundlesToLoad)
            {
                List<string> clipNames = new();
                var findClipNames = clipsToLoad.FindAll(c => c.bundle == path.bundle);
                for (int i = 0; i < findClipNames.Count; i++)
                {
                    clipNames.Add(findClipNames[i].clipName);
                }
                ImportSelectedSpritesFromAssetBundle(path.bundle, clipNames, path.isAddressable);
            }
        }

        internal static void DeleteCachedIfMissing()
        {
            var missingBundles = new List<string>();
            foreach (var asset in customAssets)
            {
                var filePath = asset.filePath.Replace("?", "");
                if (asset.customAssetFileType == CustomAssetFileType.ImageFile)
                    continue;
                if (!asset.filePath.Contains("?"))
                    continue;
                if (missingBundles.Exists(a => a == filePath))
                    continue;
                missingBundles.Add(filePath);
            }
            foreach (var path in missingBundles)
            {
                var pathName = Path.GetFileNameWithoutExtension(path);
                var directory = Path.Combine(cachedAnimPath, pathName);
                if (Directory.Exists(directory))
                {
                    var files = Directory.GetFiles(directory);
                    for (int i = files.Length - 1; i >= 0; i--)
                        File.Delete(files[i]);
                    Directory.Delete(directory);
                }
            }
        }

        internal static void CheckForMissingFilesAtStart()
        {
            var missingAssets = new List<CustomAsset>();
            foreach (var asset in customAssets)
            {
                if (!File.Exists(asset.filePath) && !missingAssets.Exists(a => CustomAsset.IsCustomAssetTheSame(asset, a)))
                    missingAssets.Add(asset);
            }
            if (missingAssets.Count <= 0)
                return;
            PromptMissingFiles(missingAssets, ref customAssets);
            DeleteCachedIfMissing();
            SaveCustomAssets();
        }

        internal static void LoadCustomAssets()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.CustomAssetSettings))
                return;
            var settings = JsonConvert.DeserializeObject<CustomAssetSettings>(Properties.Settings.Default.CustomAssetSettings);
            catalogPath = settings.catalogPath;
            customAssets = settings.customAssets;
        }

        internal static void SaveCustomAssets()
        {
            CustomAssetSettings customAssetSettings = new();
            customAssetSettings.customAssets = customAssets;
            customAssetSettings.catalogPath = catalogPath;
            var json = JsonConvert.SerializeObject(customAssetSettings, Formatting.Indented);
            Properties.Settings.Default.CustomAssetSettings = json;
            Properties.Settings.Default.Save();
        }
        internal static bool InitializeCatalogPath()
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = string.IsNullOrEmpty(Properties.Settings.Default.BundleDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : Properties.Settings.Default.BundleDirectory;
            openNsoStream.Filter = "JSON File (*.json)|*.json";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.BundleDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                if (string.IsNullOrEmpty(openNsoStream.FileName))
                    return false;
                var catalog = File.ReadAllText(openNsoStream.FileName);
                if (!catalog.Contains(VALID_CATALOG_STRING))
                {
                    MessageBox.Show("Catalog is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                var check = CheckIfAddressablesMatchCatalog(openNsoStream.FileName);
                if (check)
                {
                    var confirm = MessageBox.Show("Some addressables rely on the current catalog. By changing the catalog, some of these addressables will be deleted. \n\nChange the current catalog?", "Wait!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm != DialogResult.Yes)
                        return false;
                }
                catalogPath = openNsoStream.FileName;
                if (check)
                    DeleteUnmatchingAddressables();
                SaveCustomAssets();
            }
            return true;
        }

        private static bool CheckIfAddressablesMatchCatalog(string catalogPath)
        {
            if (string.IsNullOrEmpty(catalogPath)) return false;
            var catalog = File.ReadAllText(catalogPath);
            bool isAddressableChanged = false;
            foreach (var bundle in customAssets)
            {
                if (bundle.customAssetFileType != CustomAssetFileType.AddressableBundle)
                    continue;
                var path = Path.GetFileName(bundle.filePath);
                if (!catalog.Contains(path))
                    isAddressableChanged = true;
                break;
            }
            return isAddressableChanged;
        }

        private static void DeleteUnmatchingAddressables()
        {
            var catalog = File.ReadAllText(catalogPath);
            foreach (var bundle in customAssets)
            {
                if (bundle.customAssetFileType != CustomAssetFileType.AddressableBundle)
                    continue;
                var path = Path.GetFileName(bundle.filePath);
                if (!catalog.Contains(path))
                    customAssets.Remove(bundle);
            }
            CheckForEmptyAnimFolder();
        }

        internal static void CreateCustomFolder()
        {
            if (!Directory.Exists(cachedAnimPath))
            {
                Directory.CreateDirectory(cachedAnimPath);
            }
        }

        internal static void CheckForEmptyAnimFolder()
        {
            var directories = Directory.GetDirectories(cachedAnimPath);
            for (int i = 0; i < directories.Length; i++)
            {
                var files = Directory.GetFiles(directories[i]);
                if (files.Length <= 1)
                {
                    if (files.Length == 1)
                    {
                        File.Delete(files[0]);
                    }
                    Directory.Delete(directories[i]);
                }
            }
        }

        internal static void ImportSpriteFromAddressable(bool isEncodedLz4)
        {
            if (catalogPath == null)
            {
                var confirm = MessageBox.Show("Catalog for Addressables hasn't been initialized yet. Do you want to set the catalog now?", "Wait!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (!InitializeCatalogPath())
                        return;
                }
                else return;

            }
            ImportSpriteFromAssetBundle(isEncodedLz4, true);
        }

        internal static void ImportSelectedSpritesFromAssetBundle(string path, List<string> animNames, bool isAddressable = false)
        {
            var otherPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
            try
            {
                var am = new AssetsManager();
                var bunInst = am.LoadBundleFile(path, true);
                var assInst = am.LoadAssetsFileFromBundle(bunInst, 0, true);
                var bundleName = Path.GetFileNameWithoutExtension(path);
                var pathToCacheBundle = Path.Combine(otherPath, bundleName);
                if (!Directory.Exists(pathToCacheBundle))
                    Directory.CreateDirectory(pathToCacheBundle);
                for (int i = 0; i < animNames.Count; i++)
                {
                    foreach (var picInfo in assInst.file.GetAssetsOfType(AssetClassID.Texture2D))
                    {
                        var pic = am.GetBaseField(assInst, picInfo);
                        if (!pic["m_Name"].AsString.Contains(animNames[i]))
                            continue;
                        if (customAssets.Exists(a => a.fileName.Contains(animNames[i])))
                            continue;
                        var tf = TextureFile.ReadTextureFile(pic);
                        var texDat = tf.GetTextureData(assInst);
                        using (var file = File.Create(Path.Combine(pathToCacheBundle, animNames[i])))
                        {
                            file.Write(texDat, 0, texDat.Length);
                        }
                        var importedAnimInfo = new CustomAsset(CustomAssetType.Sprite, isAddressable ? CustomAssetFileType.AddressableBundle : CustomAssetFileType.AssetBundle, animNames[i], path);
                        importedAnimInfo.picWidth = pic["m_Width"].AsInt;
                        importedAnimInfo.picHeight = pic["m_Height"].AsInt;
                        customAssets.Add(importedAnimInfo);
                        break;

                    }
                }
                File.WriteAllText(Path.Combine(pathToCacheBundle, "bundlePath.txt"), path + (isAddressable ? $"\ncatalog: {catalogPath}" : ""));
                SaveCustomAssets();
            }
            catch { MessageBox.Show("Could not load asset bundle, either the file is corrupt or is not supported.", "Could not read asset bundle", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
        internal static void ImportSpriteFromAssetBundle(bool isEncodedLz4, bool isAddressable = false)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CustomStreamMaker\CustomAnimationClips";
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = string.IsNullOrEmpty(Properties.Settings.Default.BundleDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : Properties.Settings.Default.BundleDirectory;
            openNsoStream.Filter = "All Files (*.*)|*.*";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.BundleDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                if (isAddressable)
                {
                    var catalog = File.ReadAllText(catalogPath);
                    if (!catalog.Contains(Path.GetFileName(openNsoStream.FileName)))
                    {
                        MessageBox.Show("Catalog does not contain this addressable.", "Addressable not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                try
                {
                    var am = new AssetsManager();
                    var bunInst = am.LoadBundleFile(openNsoStream.FileName, true);
                    var assInst = am.LoadAssetsFileFromBundle(bunInst, 0, true);
                    var bundleName = Path.GetFileNameWithoutExtension(openNsoStream.FileName);
                    var pathToCacheBundle = Path.Combine(path, bundleName);
                    List<string> animNames = new();
                    foreach (var info in assInst.file.GetAssetsOfType(AssetClassID.AnimationClip))
                    {
                        var anim = am.GetBaseField(assInst, info);
                        if (!(ThatOneLongListOfAnimationsOriginallyInTheGame.list.Contains(anim["m_Name"].AsString) || ThatOneLongListOfAnimationsOriginallyInTheGame.forbidden.Contains(anim["m_Name"].AsString)))
                            animNames.Add(anim["m_Name"].AsString);
                    }
                    if (animNames.Count == 0)
                    {
                        MessageBox.Show("Could not find any animation clips from the bundle.", "No animation clips found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (animNames.Count > 1)
                    {
                        var clips = new AnimationClipList(animNames);
                        clips.OnClosingConfirm += () => { animNames = clips.animList; };
                        clips.ShowDialog();
                    }
                    if (animNames.Count == 0)
                    {
                        MessageBox.Show("Did not import any animation clips as no clips were selected.");
                        return;
                    }
                    if (!Directory.Exists(pathToCacheBundle))
                        Directory.CreateDirectory(pathToCacheBundle);
                    for (int i = 0; i < animNames.Count; i++)
                    {
                        foreach (var picInfo in assInst.file.GetAssetsOfType(AssetClassID.Texture2D))
                        {
                            var pic = am.GetBaseField(assInst, picInfo);
                            if (!pic["m_Name"].AsString.Contains(animNames[i]))
                                continue;
                            if (customAssets.Exists(a => a.fileName.Contains(animNames[i]) && a.filePath != openNsoStream.FileName))
                            {
                                var oldAsset = customAssets.Find(a => a.fileName.Contains(animNames[i]) && a.filePath != openNsoStream.FileName);
                                customAssets.Remove(oldAsset);
                            }
                            else if (customAssets.Exists(a => a.fileName.Contains(animNames[i])))
                                continue;
                            var tf = TextureFile.ReadTextureFile(pic);
                            var texDat = tf.GetTextureData(assInst);
                            using (var file = File.Create(Path.Combine(pathToCacheBundle, animNames[i])))
                            {
                                file.Write(texDat, 0, texDat.Length);
                            }
                            var importedAnimInfo = new CustomAsset(CustomAssetType.Sprite, isAddressable ? CustomAssetFileType.AddressableBundle : CustomAssetFileType.AssetBundle, animNames[i], openNsoStream.FileName);
                            importedAnimInfo.picWidth = pic["m_Width"].AsInt;
                            importedAnimInfo.picHeight = pic["m_Height"].AsInt;
                            customAssets.Add(importedAnimInfo);
                            break;
                        }
                    }
                    File.WriteAllText(Path.Combine(pathToCacheBundle, "bundlePath.txt"), openNsoStream.FileName + (isAddressable ? $"\ncatalog: {catalogPath}" : ""));
                    SaveCustomAssets();
                    MessageBox.Show("Import successful!");
                }
                catch { MessageBox.Show("Could not load asset bundle, either the file is corrupt or is not supported.", "Could not read asset bundle", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        internal static bool CheckIfClipExists(string path, string clipName, out string message)
        {
            try
            {
                message = string.Empty;
                bool foundAsset = false;
                var am = new AssetsManager();
                var bunInst = am.LoadBundleFile(path, true);
                var assInst = am.LoadAssetsFileFromBundle(bunInst, 0, true);
                var bundleName = Path.GetFileNameWithoutExtension(path);
                var pathToCacheBundle = Path.Combine(path, bundleName);
                List<string> animNames = new();
                foreach (var info in assInst.file.GetAssetsOfType(AssetClassID.AnimationClip))
                {
                    var anim = am.GetBaseField(assInst, info);
                    if (anim["m_Name"].AsString == clipName)
                    {
                        foundAsset = true;
                        break;
                    }
                }
                if (!foundAsset)
                {
                    message = "Could not find any matching animation clips from the bundle.";
                    return false;
                }
                return true;
            }
            catch
            {
                message = "Could not load asset bundle, either the file is corrupt or is not supported.";
            }
            return false;
        }


        internal static bool CheckIfImageFileExists(string path, out string message)
        {
            message = string.Empty;
            try
            {
                var data = File.ReadAllBytes(path);
                var format = Image.DetectFormat(data);
                if (format == null || !(format is PngFormat || format is JpegFormat))
                {
                    message = "Could not load image file, only PNG's and JPEG's are supported.";
                    return false;
                }
                return true;
            }
            catch { message = "Could not load image file, either the image file is corrupt or is not supported."; }
            return false;
        }

        internal static void ImportImage(bool isEndScreen = false)
        {
            OpenFileDialog openNsoStream = new OpenFileDialog();
            openNsoStream.InitialDirectory = string.IsNullOrEmpty(Properties.Settings.Default.ImageDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) : Properties.Settings.Default.ImageDirectory;
            openNsoStream.Filter = "png File (*.png)|*.png|jpg File (*.jpg)|*.jpg";
            openNsoStream.FilterIndex = 1;
            openNsoStream.RestoreDirectory = true;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.ImageDirectory = Path.GetDirectoryName(openNsoStream.FileName);
                try
                {
                    var data = File.ReadAllBytes(openNsoStream.FileName);
                    var format = Image.DetectFormat(data);
                    if (format == null || !(format is PngFormat || format is JpegFormat))
                    {
                        MessageBox.Show("Could not load image file, only PNG's and JPEG's are supported.", "Unsupported file type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var fileName = Path.GetFileNameWithoutExtension(openNsoStream.FileName);
                    var importedImageInfo = new CustomAsset(isEndScreen ? CustomAssetType.EndScreen : CustomAssetType.Background, CustomAssetFileType.ImageFile, fileName, openNsoStream.FileName);
                    if (customAssets.Exists(a => a.filePath == importedImageInfo.filePath))
                    {
                        MessageBox.Show("Could not create asset, this custom asset already exists.", "Could not create new asset", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    customAssets.Add(importedImageInfo);
                    SaveCustomAssets();
                    MessageBox.Show("Import successful!");
                }
                catch { MessageBox.Show("Could not load image file, either the image file is corrupt or is not supported.", "Could not read image file", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }


}
