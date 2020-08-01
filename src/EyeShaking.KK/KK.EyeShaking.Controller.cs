﻿using ExtensibleSaveFormat;
using KKAPI;
using KKAPI.Chara;
using KKAPI.Studio;

namespace KK_Plugins
{
    public partial class EyeShaking
    {
        public class EyeShakingController : CharaCustomFunctionController
        {
            private bool _EyeShaking = false;
            public bool EyeShaking
            {
                get => _EyeShaking;
                set
                {
                    _EyeShaking = value;
                    ChaControl.ChangeEyesShaking(value);
                }
            }
            internal bool IsVirgin { get; set; } = true;
            internal bool IsVirginOrg { get; set; } = true;
            internal bool IsInit { get; set; } = false;

            protected override void OnCardBeingSaved(GameMode currentGameMode)
            {
                if (!StudioAPI.InsideStudio) return;

                var data = new PluginData();
                data.data.Add("EyeShaking", EyeShaking);
                SetExtendedData(data);
            }
            protected override void OnReload(GameMode currentGameMode, bool maintainState)
            {
                _EyeShaking = false;

                if (!StudioAPI.InsideStudio) return;

                var data = GetExtendedData();
                if (data != null && data.data.TryGetValue("EyeShaking", out var loadedEyeShakingState))
                    EyeShaking = (bool)loadedEyeShakingState;
            }

            internal void HSceneStart(bool virgin)
            {
                IsVirgin = virgin;
                IsVirginOrg = virgin;
                IsInit = true;
            }

            internal void HSceneEnd()
            {
                EyeShaking = false;
                IsInit = false;
            }

            internal void OnInsert() => IsVirgin = false;
            internal void AddOrgasm() => IsVirginOrg = false;

            protected override void Update()
            {
                if (Enabled.Value && IsInit && (IsVirgin || IsVirginOrg))
                    EyeShaking = true;
            }
        }
    }
}