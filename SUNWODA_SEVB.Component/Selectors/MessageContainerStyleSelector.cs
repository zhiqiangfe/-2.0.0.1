using SUNWODA_SEVB.Component.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SUNWODA_SEVB.Component.Selectors
{
    /// <summary>
    /// 消息项容器样式选择器
    /// </summary>
    public class MessageContainerStyleSelector : StyleSelector
    {
        public Style UserMessageStyle { get; set; }
        public Style AssistantMessageStyle { get; set; }
        public Style SystemMessageStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ChatMessageViewModel message)
            {
                switch (message.Role)
                {
                    case MessageRole.User:
                        return UserMessageStyle ?? base.SelectStyle(item, container);
                    case MessageRole.Assistant:
                        return AssistantMessageStyle ?? base.SelectStyle(item, container);
                    case MessageRole.System:
                        return SystemMessageStyle ?? base.SelectStyle(item, container);
                }
            }

            return base.SelectStyle(item, container);
        }
    }

    /// <summary>
    /// 消息数据模板选择器
    /// </summary>
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserMessageTemplate { get; set; }
        public DataTemplate AssistantMessageTemplate { get; set; }
        public DataTemplate SystemMessageTemplate { get; set; }
        public DataTemplate ThinkingMessageTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ChatMessageViewModel message)
            {
                // 如果有思考步骤，使用思考消息模板
                if (message.ThinkingSteps?.Count > 0)
                {
                    return ThinkingMessageTemplate ?? SystemMessageTemplate;
                }

                switch (message.Role)
                {
                    case MessageRole.User:
                        return UserMessageTemplate;
                    case MessageRole.Assistant:
                        return AssistantMessageTemplate;
                    case MessageRole.System:
                        return SystemMessageTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
