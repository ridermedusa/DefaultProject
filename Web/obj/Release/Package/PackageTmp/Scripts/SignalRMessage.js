//开始定义对应解析
var SignalRMessage = {
    //获取日志类型数据
    Log: function (Message) {
        return SignalRMessage.ReturnMessage(Message, "Log");
    },
    //获取返回百分比类型数据
    Percentage: function (Message) {
        return SignalRMessage.ReturnMessage(Message, "Percentage");
    },
    ReturnMessage: function (Message,Type) {
        try {
            //console.log(Type + ":" + Message.Type);
            if (Message.Type == Type) {
                return Message.Body;
            }
            else {
                return "";
            }
        } catch (e) {
            console.log(e);
            return "";
        }
    }
}