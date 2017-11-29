namespace Indspire.Soaring.Engagement.ViewModels
{
    using Indspire.Soaring.Engagement.Database;

    public class AwardScanViewModel
    {
        public string AwardNumber { get; set; }
        public bool HasAwardNumber { get; set; }
        public bool AwardNumberInvalid { get; set; }
        public Award Award { get; set; }
    }
}
