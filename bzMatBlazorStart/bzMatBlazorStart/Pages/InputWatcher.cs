﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bzMatBlazorStart.Pages
{
    public class InputWatcher : ComponentBase
    {
        private EditContext editContext;

        [CascadingParameter]
        protected EditContext EditContext
        {
            get => editContext;
            set
            {
                editContext = value;
                EditContextActionChanged?.Invoke(editContext);
            }
        }

        [Parameter]
        public Action<EditContext> EditContextActionChanged { get; set; }
    }
}
