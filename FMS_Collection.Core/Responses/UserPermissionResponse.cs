using FMS_Collection.Core.Common;

namespace FMS_Collection.Core.Response
{
    public class UserPermissionResponse : CommonResponse
    {
        public Guid? Id { get; set; }
        public Guid? ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? Description{ get; set; }
        public string? Route { get; set; }
        public bool? View { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? Download { get; set; }
        public bool? Upload { get; set; }
    }
}
