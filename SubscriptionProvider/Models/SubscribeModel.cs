using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionProvider.Models;

public class SubscribeModel
{
    public string Email { get; set; } = null!;

    public bool IsSubscribed { get; set; } = false;
}
