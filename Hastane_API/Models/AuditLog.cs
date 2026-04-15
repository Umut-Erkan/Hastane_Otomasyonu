using System;
using System.Collections.Generic;

namespace MyApiProject.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int Userıd { get; set; }

    public string ServiceName { get; set; }

    public string BrowserInfo { get; set; }

    public string Role { get; set; }

    public DateTime? Time { get; set; }
}
