using CMS.Common.Enum;

namespace CMS.MentApi.Untility.DatabaseExt
{
    /// <summary>
    /// 
    /// </summary>
    public class FunctionAttribute : Attribute
    {
        private MenuType _menuType { get; set; }


        private string? _menuName {  get; set; }

        private string? _webUrlName {  get; set; }

        private string? _icon { get; set; }
/// <summary>
/// 
/// </summary>
/// <param name="menuType"></param>
/// <param name="menuName"></param>
/// <param name="webUrlName"></param>
/// <param name="icon"></param>
        public FunctionAttribute(MenuType menuType, string? menuName = null, string? webUrlName=null, string?  icon= "MenuOutlined")
        {
            _menuType = menuType;
            _menuName = menuName;
            _webUrlName = webUrlName;
            _icon = icon;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MenuType GetMenuType()
        {
            return _menuType;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string? GetMenuName()
        {
            return _menuName;
        }
        /// <summary>
        /// web url for front end
        /// </summary>
        /// <returns></returns>
        public string? GetWebUrlName() { return _webUrlName; }

        /// <summary>
        /// vue project`s file path 
        /// </summary>
        /// <returns></returns>
        public string? GetIcon () => _icon;
    }

 
}
