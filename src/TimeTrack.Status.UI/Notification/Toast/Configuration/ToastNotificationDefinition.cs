using System;
using System.Collections.Generic;

namespace TimeTrack.Status.UI.Notification.Toast.Configuration
{
    public class ToastNotificationDefinition : ICloneable
    {
        public string DefinitionName { get; set; }
        public string ImageFileOrUrl { get; set; }

        public bool LongDisplayDuration { get; set; }

        public bool LoopSound { get; set; }

        public string Sound { get; set; }

        public string StandardTemplateName { get; set; }

        public IEnumerable<string> TextLines { get; set; }

        public object Clone()
        {
            return new ToastNotificationDefinition
            {
                DefinitionName = DefinitionName,
                ImageFileOrUrl = ImageFileOrUrl,
                LongDisplayDuration = LongDisplayDuration,
                LoopSound = LoopSound,
                Sound = Sound,
                StandardTemplateName = StandardTemplateName,
                TextLines = TextLines
            };
        }
    }
}