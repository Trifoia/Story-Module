using Ink;
using Microsoft.AspNetCore.Components;
using Oqtane.Interfaces;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InkRun = global::Ink.Runtime;

namespace Trifoia.Module.Story
{
    public abstract class StoryModuleBase : ModuleBase, ISettingsControl
    {
        [Inject]
        protected ISettingService SettingService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected bool _hasTextBubble = false;
        protected bool _centerJustify = true;
        protected bool _promptFillSpace = true;
        protected bool _choicesFillSpace = true;
        protected string _imageRoot = "/Themes/images/lottieplaceholder/";
        protected string _lottieRoot = "/Themes/lottie/";
        protected string _ink;
        protected string _inkJSON;
        protected bool _returnOnComplete = true;

        // results of compiling the ink script
        protected InkRun.Story _story;
        protected string _errorMessage;

        protected bool _busy;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                string str;

                var settings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);

                str = SettingService.GetSetting(settings, "HasTextBubble", "false");
                _hasTextBubble = Boolean.Parse(str);

                str = SettingService.GetSetting(settings, "CenterJustify", "true");
                _centerJustify = Boolean.Parse(str);

                str = SettingService.GetSetting(settings, "PromptFillSpace", "true");
                _promptFillSpace = Boolean.Parse(str);

                str = SettingService.GetSetting(settings, "ChoicesFillSpace", "true");
                _choicesFillSpace = Boolean.Parse(str);

                _imageRoot = SettingService.GetSetting(settings, "InkImageRoot", _imageRoot);
             
                _ink = SettingService.GetSetting(settings, "Ink", "");
                _inkJSON = SettingService.GetSetting(settings, "InkJSON", "");

                var returnOnCompleteSetting = SettingService.GetSetting(settings, "ReturnOnComplete", "true");
                _returnOnComplete = Boolean.Parse(returnOnCompleteSetting);


                CompileStory();
            }
            catch (Exception e)
            {
                // TODO: log exceptions
                Console.Write(e.Message);
            }
        }

        #region Edit Settings

        public async Task UpdateSettings()
        {
            try
            {
                _busy = true;
                StateHasChanged();
                await Task.Delay(100);

                Dictionary<string, string> settings = await SettingService.GetModuleSettingsAsync(ModuleState.ModuleId);

                SettingService.SetSetting(settings, "Ink", _ink);
                SettingService.SetSetting(settings, "InkJSON", _inkJSON);
                SettingService.SetSetting(settings, "HasTextBubble", _hasTextBubble.ToString());
                SettingService.SetSetting(settings, "CenterJustify", _centerJustify.ToString());
                SettingService.SetSetting(settings, "PromptFillSpace", _promptFillSpace.ToString());
                SettingService.SetSetting(settings, "ChoicesFillSpace", _choicesFillSpace.ToString());
                SettingService.SetSetting(settings, "InkImageRoot", _imageRoot);
                SettingService.SetSetting(settings, "InkLottieRoot", _lottieRoot);
                SettingService.SetSetting(settings, "ReturnOnComplete", _returnOnComplete.ToString());

                await SettingService.UpdateModuleSettingsAsync(settings, ModuleState.ModuleId);

                // not necessary, just to catch any JSON errors
                CompileStory();

                _busy = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ModuleInstance.AddModuleMessage(ex.Message, Oqtane.Modules.MessageType.Error);
            }
        }

        #endregion

        protected void CompileStory()
        {
            _errorMessage = "";
                
            try
            {
                var compiler = new Compiler(_ink);

                // Kind of dumb way to see if it can compile, and to fallback onto json if not. Compiler object doesn't have a method for checking.
                try
                {
                    if (String.IsNullOrEmpty(_ink))
                    {
                        throw new Exception();
                    }

                    var compiledStory = compiler.Compile();
                    _story = compiledStory;

                    _inkJSON = _story.ToJson();
                } catch
                {
                    _story = new InkRun.Story(_inkJSON);
                }
            }
            catch (Exception e)
            {
                _story = null;
                _errorMessage = e.Message;
            }

            StateHasChanged();
        }
    }
}
