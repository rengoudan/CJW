namespace JwServices
{
    /// <summary>
    /// 2026年11月27日 顧客管理画面用：顧客が使っている工事 1 件（JwqitaService.GetCustomerProjectItemsAsync）。
    /// </summary>
    public sealed record CustomerProjectItem(long CustomerId, long ProjectId, string ProjectName, int FloorQuantity);
}
