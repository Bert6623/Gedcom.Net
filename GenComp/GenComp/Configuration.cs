using System;
using System.Configuration;

namespace GenComp {
    public static class Configuration {
        public static int SegmentMinPhasedSnpCount {
            get { return GetAppSettingsInt("SegmentMinPhasedSnpCount", 200); }
        }

        public static double SegmentMinCmLength {
            get { return GetAppSettingsDouble("SegmentMinCmLength", 3.0); }
        }

        public static int MaxErrorsToStitch {
            get { return GetAppSettingsInt("MaxErrorsToStitch", 1); }
        }

        public static int StitchMinPhasedSnpCount {
            get { return GetAppSettingsInt("StitchMinPhasedSnpCount", 50); }
        }

        public static double StitchMinCmLength {
            get { return GetAppSettingsDouble("StitchMinCmLength", 1.0); }
        }

        public static int PhaseSegmentMinSnpCount {
            get { return GetAppSettingsInt("PhaseSegmentMinSnpCount", 600); }
        }

        public static int PhaseSegmentEdgeWaste {
            get { return GetAppSettingsInt("PhaseSegmentEdgeWaste", 250); }
        }

        public static bool FillNoCalls {
            get { return GetAppSettingsBool("FillNoCalls", true); }
        }

        public static bool WriteDetailedPhaseHistory {
            get { return GetAppSettingsBool("WriteDetailedPhaseHistory", false); }
        }

        public static char UnphasedChar {
            get { return GetAppSettingsString("UnphasedChar", "-")[0]; }
        }

        public static string GenomePath {
            get {
                string result = GetAppSettingsString("GenomePath", "");
                if (string.IsNullOrWhiteSpace(result)) {
                    result = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                }
                return result;
            }
        }

        private static string GetAppSettingsString(string name, string defaultValue) {
            string setting = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrEmpty(setting)) setting = defaultValue;
            return setting;
        }

        private static int GetAppSettingsInt(string name, int defaultValue) {
            int result;
            string setting = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrEmpty(setting) || !int.TryParse(setting, out result)) {
                result = defaultValue;
            }
            return result;
        }

        private static double GetAppSettingsDouble(string name, double defaultValue) {
            double result;
            string setting = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrEmpty(setting) || !double.TryParse(setting, out result)) {
                result = defaultValue;
            }
            return result;
        }

        private static bool GetAppSettingsBool(string name, bool defaultValue) {
            bool result;
            string setting = ConfigurationManager.AppSettings[name];
            if (String.IsNullOrEmpty(setting) || !bool.TryParse(setting, out result)) {
                result = defaultValue;
            }
            return result;
        }
    }
}
