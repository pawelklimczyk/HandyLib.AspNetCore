namespace Gmtl.HandyLib.AspNetCore.Models
{
    public abstract class AbstractPageViewModel
    {
        private string _metaTitle;
        public string MetaTitle
        {
            get
            {
                return _metaTitle;
            }
            set
            {
                _metaTitle = value;
                OgTitle = _metaTitle;
            }
        }

        private string _metaDescription;
        public string MetaDescription
        {
            get
            {
                return _metaDescription;
            }
            set
            {
                _metaDescription = value;
                OgDescription = _metaDescription;
            }
        }

        public string MetaKeywords { get; set; }
        public string MetaAbstract { get; set; }
        public string MetaRobots { get; set; } = "index, follow, all";

        private string _canonicalUrl;
        public string CanonicalUrl
        {
            get
            {
                return _canonicalUrl;
            }
            set
            {
                _canonicalUrl = value;
                OgUrl = _canonicalUrl;
            }
        }

        public string OgImageUrl { get; set; }
        public string OgTitle { get; set; }
        public string OgDescription { get; set; }
        public string OgUrl { get; set; }
        public string OgType { get; set; } = "website";
    }
}
