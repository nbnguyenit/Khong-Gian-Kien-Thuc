﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.ViewModels.Systems
{
    public class CommandAssignRequest
    {
        public string[] CommandIds { get; set; }
        public bool AddToAllFunctions { get; set; }
        // public string FunctionId { get; set; }
    }
}
