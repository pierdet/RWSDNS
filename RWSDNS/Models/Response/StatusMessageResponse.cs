namespace RWSDNS.Api.Models.Response
{
    public class StatusMessageResponse<TResponseItem>
        where TResponseItem : class
    {
        public TResponseItem ResponseItem { get; set; }
        public string Message { get; set; }
        
        public StatusMessageResponse(TResponseItem responseItem, string message)
        {
            Message = message;
            ResponseItem = responseItem;
        }
    }
}