namespace CMS.MentApi.Untility.DatabaseExt
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuOrButtonAttribute : Attribute
    {
        private MenuType _menuType { get; set; }

        private string _Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuType"></param>
        /// <param name="description"></param>
        public MenuOrButtonAttribute(MenuType menuType, string description)
        {
            _menuType = menuType;
            _Description = description;
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
        public string GetDescription()
        {
            return _Description;
        }

    }

    /// <summary>
    /// denfine menu or button
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 
        /// </summary>
        Menu = 1,
        /// <summary>
        /// 
        /// </summary>
        Button = 2,
    }
}
