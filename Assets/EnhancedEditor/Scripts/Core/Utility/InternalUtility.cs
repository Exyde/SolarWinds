// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using UnityEngine;

namespace EnhancedEditor {
    /// <summary>
    /// <see cref="EnhancedEditor"/>-related general utility class.
    /// </summary>
	public static class InternalUtility {
        #region Content

        private const string Name            = "Enhanced Editor";

        /// <summary>
        /// Menu path prefix used for creating new <see cref="ScriptableObject"/>, or any other special menu.
        /// </summary>
        public const string MenuPath        = Name + "/";

        /// <summary>
        /// Menu item path used for various utilities.
        /// </summary>
        public const string MenuItemPath    = "Tools/" + MenuPath;

        /// <summary>
        /// Menu order used for creating new <see cref="ScriptableObject"/> from the asset menu.
        /// </summary>
        public const int MenuOrder          = 200;
        #endregion
    }
}
