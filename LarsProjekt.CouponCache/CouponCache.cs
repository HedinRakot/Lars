using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace LarsProjekt.CouponCache;

public class CouponCache : ICouponCache
{
    private IReadOnlyCollection<Coupon> _coupons;
    private IServiceProvider _serviceProvider;

    private readonly object _lockObject = new();
    private bool _isInitialized;
    private Task? _initTask;

    public CouponCache(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IReadOnlyCollection<Coupon>> GetCoupons()
    {
        await WaitUntilInitialized();

        return _coupons;
    }

    public Task Refresh(CancellationToken cancellationToken)
    {
        lock (_lockObject)
        {
            return _initTask ?? RefreshInternal(!_isInitialized, cancellationToken);
        }
    }

    internal async Task RefreshData(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var couponService = scope.ServiceProvider.GetRequiredService<ICouponService>();
        _coupons = await couponService.GetCoupons();
    }

    internal Task WaitUntilInitialized()
    {
        lock (_lockObject)
        {
            if (_isInitialized)
            {
                return Task.CompletedTask;
            }

            return _initTask != null ? _initTask : RefreshInternal(true, CancellationToken.None);
        }
    }

    internal async Task RefreshInternal(bool isInitializing, CancellationToken cancellationToken)
    {
        try
        {
            if (isInitializing)
            {
                _initTask = RefreshData(cancellationToken);

                await _initTask;
            }
            else
            {
                await RefreshData(cancellationToken);
            }

            _isInitialized = true;
        }
        finally
        {
            _initTask = null;
        }
    }
}
