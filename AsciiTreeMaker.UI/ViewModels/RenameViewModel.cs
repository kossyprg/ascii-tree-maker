using AsciiTreeMaker.Domain.Exceptions;
using AsciiTreeMaker.Domain.ValueObjects;
using AsciiTreeMaker.UI.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace AsciiTreeMaker.UI.ViewModels
{
    public class RenameViewModel : BindableBase, IDialogAware
    {
        private IMessageService _messageService;

        public RenameViewModel() : this(new MessageService())
        {
        }

        public RenameViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            OKButton = new DelegateCommand(OKButtonExecute);
            CancelButton = new DelegateCommand(CancelButtonExecute);
        }

        public string Title => "ノード名の編集";

        private string _editText = string.Empty;
        public string EditText
        {
            get { return _editText; }
            set { SetProperty(ref _editText, value); }
        }

        public event Action<IDialogResult>? RequestClose;

        public DelegateCommand OKButton { get; }
        public DelegateCommand CancelButton { get; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            // 初期値を受け取る
            EditText = parameters.GetValue<string>(nameof(EditText)) ?? "";
        }

        public void OKButtonExecute()
        {
            NodeName nodeName;
            try
            {
                nodeName = new NodeName(EditText);
            }
            catch (NodeNameIsEmptyException ex)
            {
                // ノード名が空欄あるいは空白文字
                _messageService.Error(ex.Message);
                return;
            }
            catch (NodeNameTooLongException ex)
            {
                // ノード名が最大長を超えている
                _messageService.Error(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                _messageService.Error(ex.Message);
                return;
            }

            var p = new DialogParameters
            {
                { nameof(EditText), nodeName }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public void CancelButtonExecute()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
    }
}
