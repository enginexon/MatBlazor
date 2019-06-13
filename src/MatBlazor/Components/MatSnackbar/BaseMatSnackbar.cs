﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MatBlazor
{
    /// <summary>
    /// Snackbars provide brief messages about app processes at the bottom of the screen.
    /// </summary>
    public class BaseMatSnackbar : BaseMatDomComponent
    {
        private bool _isOpen;

        [Parameter]
        protected RenderFragment ChildContent { get; set; }

        [Parameter]
        protected bool Stacked { get; set; }

        [Parameter]
        protected bool Leading { get; set; }

        [Parameter]
        protected bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (IsOpen != value)
                {
                    _isOpen = value;
                    CallAfterRender(async () =>
                    {
                        await Js.InvokeAsync<object>("matBlazor.matSnackbar.setIsOpen", Ref, value);
                    });
                }
            }
        }

        [Parameter]
        public EventCallback<bool> IsOpenChanged { get; set; }

        public BaseMatSnackbar()
        {
            ClassMapper
                .Add("mdc-snackbar")
                .If("mdc-snackbar--stacked", () => Stacked)
                .If("mdc-snackbar--leading", () => Leading);
            CallAfterRender(async () =>
            {
                await Js.InvokeAsync<object>("matBlazor.matSnackbar.init", Ref, DotNetObjectRef.Create(this));
            });
        }

        [JSInvokable]
        public async Task MatSnackbarClosedHandler()
        {
            _isOpen = false;
            await IsOpenChanged.InvokeAsync(false);
            this.StateHasChanged();
        }
    }
}