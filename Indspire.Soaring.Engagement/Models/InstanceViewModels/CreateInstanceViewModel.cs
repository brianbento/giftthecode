namespace Indspire.Soaring.Engagement.Models.InstanceViewModels
{
    using System.ComponentModel;

    public class CreateInstanceViewModel
    {
        [DisplayName("Event Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Default Instance")]
        public bool DefaultInstance { get; set; }
    }
}
