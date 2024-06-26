﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionProvider.Data.Entities;

public class SubscribeEntity
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public bool IsSubscribed { get; set; } = false;
}
