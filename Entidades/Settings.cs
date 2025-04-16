using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Settings
    {
        #region Propiedades
        public int Mode { get; set; }
        public int Theme { get; set; }
        public bool CardAnimatedCover {  get; set; }
        public bool CardSkipSongs { get; set; }
        public bool CardBlurredCoverAsBackground {  get; set; }
        public int PrivacyVisSavedSongs { get; set; }
        public int PrivacyVisStats {  get; set; }
        public int PrivacyVisFol {  get; set; }
        public bool PrivateAccount {  get; set; }
        public String Language { get; set; }
        public bool AudioLoop { get; set; }
        public bool AudioAutoPlay { get; set; }
        public bool AudioOnlyAudio { get; set; }
        public bool Notifications { get; set; }
        public bool NotiFriendsRequest { get; set; }
        public bool NotiFriendsApproved { get; set; }
        public bool NotiAppUpdate { get; set; }
        public bool NotiAppRecap { get; set; }
        public bool NotiAccountBlocked { get; set; }
        #endregion

        #region Constructores
        public Settings() { }

        public Settings(
            int mode,
            int theme,
            bool cardAnimatedCover,
            bool cardSkipSongs,
            bool cardBlurredCoverAsBackground,
            int privacyVisSavedSongs,
            int privacyVisStats,
            int privacyVisFol,
            bool privateAccount,
            string language,
            bool audioLoop,
            bool audioAutoPlay,
            bool audioOnlyAudio,
            bool notifications,
            bool notiFriendsRequest,
            bool notiFriendsApproved,
            bool notiAppUpdate,
            bool notiAppRecap,
            bool notiAccountBlocked
        )
        {
            Mode = mode;
            Theme = theme;
            CardAnimatedCover = cardAnimatedCover;
            CardSkipSongs = cardSkipSongs;
            CardBlurredCoverAsBackground = cardBlurredCoverAsBackground;
            PrivacyVisSavedSongs = privacyVisSavedSongs;
            PrivacyVisStats = privacyVisStats;
            PrivacyVisFol = privacyVisFol;
            PrivateAccount = privateAccount;
            Language = language;
            AudioLoop = audioLoop;
            AudioAutoPlay = audioAutoPlay;
            AudioOnlyAudio = audioOnlyAudio;
            Notifications = notifications;
            NotiFriendsRequest = notiFriendsRequest;
            NotiFriendsApproved = notiFriendsApproved;
            NotiAppUpdate = notiAppUpdate;
            NotiAppRecap = notiAppRecap;
            NotiAccountBlocked = notiAccountBlocked;
        }
        #endregion
    }
}
