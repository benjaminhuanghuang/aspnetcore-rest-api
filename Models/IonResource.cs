namespace aspnetcore_rest_api.Models
{
    public abstract class IonResource
    {
        [JsonProperty]
        public IonLink Meta {get;set;}
        
    }
}