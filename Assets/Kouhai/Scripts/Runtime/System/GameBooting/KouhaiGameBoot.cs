using System.Collections;
using Kouhai.Core.AssetManagement;
using Kouhai.Scripting.Interpretter;
using Kouhai.Scripts.Runtime.System.Notification;
using UnityEngine;

namespace Kouhai.Runtime.System
{
    public class KouhaiGameBoot:MonoBehaviour
    {
        private const string GAME_ENTRYPOINT_SCRIPT = "Scripts/game";
        private void Start()
        {
            KouhaiLoadingScreen.Instance.Show(
                KouhaiAppLocalisation.Current.GetLocalisedText(
                    KouhaiAppLocalisation.LocalisationTextType.BootLoadingScreen_1),
                    StartLoadingKouhaiSceneAssets);
        }
        
        private void OnDestroy()
        {
            KouhaiAssetManager.UnloadStoryPack(null);
        }

        private void StartLoadingKouhaiSceneAssets()
        {
            KouhaiLoadingScreen.Instance.ShowWaitingAniamtion();
#if UNITY_EDITOR && !KOUHAI_APP_TESTING
            StartCoroutine(OnAssetsLoaded());
#else
            LoadStoryPack();
#endif
        }

        private void LoadStoryPack()
        {
            var packName = "com.Kouhai.Kouhai Project.0_1_dev";
            KouhaiAssetManager.LoadStoryPack(packName, (success) =>
            {
                if (success)
                {
                    StartCoroutine(OnAssetsLoaded());
                }
                else
                {
                    var title = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                        .LocalisationTextType.ErrorEncountered);
                    var desc = string.Format(KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                        .LocalisationTextType.BootLoadingScreenStoryPackErr), packName);
                    var ok = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                        .LocalisationTextType.Ok);
                    KouhaiNotificationManager.ShowDialogOk(KouhaiDialogPopup.DialogType.ERROR, title, desc,ok,
                        () =>
                        {
                            Destroy(this.gameObject);
                        });
                }
            });
        }

        private IEnumerator OnAssetsLoaded()
        {
            yield return new WaitForSeconds(1f);
            KouhaiLoadingScreen.Instance.SetText(
                KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation.LocalisationTextType.BootLoadingScreen_2));
            yield return new WaitForSeconds(1);
            KouhaiLoadingScreen.Instance.Hide(SetupGameSystems);
        }

        private void SetupGameSystems()
        {
            var go = Resources.Load<GameObject>(KouhaiResourcesPath.KouhaiSystem);
            var instance = Instantiate(go);

            var entryPoint = KouhaiAssetManager.LoadAsset<KouhaiLuaScript>(GAME_ENTRYPOINT_SCRIPT);
            if (entryPoint == null)
            {
                var title = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                    .LocalisationTextType.ErrorEncountered);
                var desc = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                    .LocalisationTextType.EntryPointMissing);
                var ok = KouhaiAppLocalisation.Current.GetLocalisedText(KouhaiAppLocalisation
                    .LocalisationTextType.Ok);
                KouhaiNotificationManager.ShowDialogOk(KouhaiDialogPopup.DialogType.ERROR, title, desc,ok,
                    () =>
                    {
                        Destroy(instance.gameObject);
                        Destroy(this.gameObject);
                    });
                return;
            }
            else
            {
                var epGo = new GameObject("Game Entry Point");
                var klsc = epGo.AddComponent<KouhaiScript>();
                klsc.SetSource(entryPoint);
            }
        }
    }
}
