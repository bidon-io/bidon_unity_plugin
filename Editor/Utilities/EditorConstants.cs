// ReSharper Disable CheckNamespace
namespace Bidon.Mediation.Editor.Utilities
{
    public static class EditorConstants
    {
        public const string ProjectAndroidDirectory = "Assets/Plugins/Android";

        public const string PluginAndroidLibName = "bidon.androidlib";
        public const string PluginPrefsFileName = "PluginPreferences.asset";
        public const string PluginEditorDirectory = "Assets/Bidon/Editor";
        public const string PluginDepsDirectory = PluginEditorDirectory + "/Dependencies";

        public const string PackageName = "org.bidon.mediation";
        public const string PackageDepsSearchPattern = "*Dependencies.txt";
        public const string PackageRootDirectory = "Packages/" + PackageName;
        public const string PackageAndroidLibPath = "Runtime/Plugins/Android/" + PluginAndroidLibName + "~";
        public const string PackageDepsDirectory = PackageRootDirectory + "/Editor/Dependencies";
        public const string PackageRemoveListFilePath = PackageRootDirectory + "/Editor/PluginRemover/remove_list.xml";

        public const string PluginDocumentationLink = "https://docs.bidon.org/docs/sdk/unity/get-started";
    }
}
