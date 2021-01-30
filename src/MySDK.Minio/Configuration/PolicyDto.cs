using System.Collections.Generic;

namespace MySDK.Minio.Configuration
{
    internal class PolicyDto
    {
        public string Version => "2012-10-17";

        public List<StatementDto> Statement { get; set; }
    }

    internal class StatementDto
    {
        public string Effect => "Allow";

        public List<string> Action { get; set; }

        public List<string> Resource { get; set; }

        public PrincipalDto Principal { get; set; }
    }

    internal class PrincipalDto
    {
        public List<string> AWS => new List<string> { "*" };
    }
}
