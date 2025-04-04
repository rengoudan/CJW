using Flurl.Http;
using JwShapeCommon.JwService.Dtos;
using JwShapeCommon.JwService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JwShapeCommon.JwService
{
    public class JwApiClient: IDisposable
    {
        public static JwApiClient _apiclient;

        private static FlurlClient _client;

        public static int? TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// get token
        /// </summary>
        private const string LoginUrlSegment = "api/TokenAuth/Authenticate";

        /// <summary>
        /// refresh token
        /// </summary>
        private const string RefreshTokenUrlSegment = "api/TokenAuth/RefreshToken";

        public AbpAuthenticateResultModel AuthenticateResult { get; set; }

        public bool IsLogin { get; set; }

        /// <summary>
        /// 单例
        /// </summary>
        /// <returns></returns>
        public static JwApiClient GetClient()
        {
            if (_apiclient == null)
            {
                _apiclient = new JwApiClient();
            }
            if (!string.IsNullOrEmpty(JwConsts.LastToken))
            {
                _apiclient.AuthenticateResult = new AbpAuthenticateResultModel();
                _apiclient.AuthenticateResult.AccessToken = JwConsts.LastToken;
            }
            return _apiclient;
        }

        public static JwApiClient GetClientByUrl(string baseurl)
        {
            if (_apiclient == null)
            {
                _apiclient = new JwApiClient();
            }
            return _apiclient;
        }
        public JwApiClient() { }

        public FlurlClient GetClient(string accessToken)
        {
            if (_client == null)
            {
                CreateClient();
            }

            AddHeaders(accessToken);
            return _client;
        }

        #region post


        public async Task<T> PostAnonymousAsync<T>(string endpoint)
        {
            return await PostAsync<T>(endpoint, null, null, null, true);
        }

        public async Task<T> PostAsync<T>(string endpoint, object inputDto, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            //var httpResponse =await GetClient(accessToken)
            //    .Request(endpoint)sssssssssssssss
            //    .SetQueryParams(queryParameters)
            //    .PostJsonAsync(inputDto);
            ////return httpResponse;ReceiveJson<T>()
            ////ReceiveJson<AjaxResponse<AbpAuthenticateResultModel>>();
            //return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .PostJsonAsync(inputDto);

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        public async Task PostAsync(string endpoint, object inputDto)
        {
            await PostAsync(endpoint, inputDto, null, AuthenticateResult.AccessToken, true);
        }

        public async Task PostAsync(string endpoint, object inputDto, object queryParameters)
        {
            await PostAsync(endpoint, inputDto, queryParameters, AuthenticateResult.AccessToken, true);
        }

        public async Task PostAsync(string endpoint, object inputDto, object queryParameters, string accessToken,
            bool stripAjaxResponseWrapper)
        {
            await PostAsync<object>(endpoint, inputDto, queryParameters, accessToken, stripAjaxResponseWrapper);
        }
        #endregion

        #region get

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await GetAsync<T>(endpoint, null);
        }

        /// <summary>
        /// Makes GET request without authentication token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<T> GetAnonymousAsync<T>(string endpoint)
        {
            return await GetAsync<T>(endpoint, null, null, true);
        }

        public async Task<T> GetAsync<T>(string endpoint, object queryParameters)
        {
            return await GetAsync<T>(endpoint, queryParameters, AuthenticateResult.AccessToken, true);
        }

        public async Task<T> GetAsync<T>(string endpoint, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .GetAsync();

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        public async Task<AjaxResponse<T>> GetAjaxResponse<T>(string endpoint, object queryParameters,string accessToken)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .GetAsync();
            AjaxResponse<T> response;
            try
            {
                response = await httpResponse.ReceiveJson<AjaxResponse<T>>();
            }
            catch (FlurlHttpException e)
            {
                response = await e.GetResponseJsonAsync<AjaxResponse<T>>();
            }
            return response;
        }

        #endregion

        private static async Task<T> ValidateAbpResponse<T>(Task<IFlurlResponse> httpResponse,
            bool stripAjaxResponseWrapper)
        {
            if (!stripAjaxResponseWrapper)
            {
                return await httpResponse.ReceiveJson<T>();
            }

            AjaxResponse<T> response;
            try
            {
                response = await httpResponse.ReceiveJson<AjaxResponse<T>>();
            }
            catch (FlurlHttpException e)
            {
                response = await e.GetResponseJsonAsync<AjaxResponse<T>>();
            }

            if (response == null)
            {
                return default;
            }

            if (response.Success)
            {
                return response.Result;
            }

            if (response.Error == null)
            {
                return response.Result;
            }
            else
            {
                if (GlobalEvent.GetGlobalEvent().ApiErrorEvent != null)
                {
                    GlobalEvent.GetGlobalEvent().ApiErrorEvent(response, new ApiErrorArgs { Error = response.Error });
                }
                return response.Result;
            }
           
            //throw new Exception("");

        }

        private static void CreateClient()
        {
            _client = new FlurlClient(JwConsts.ServerUrl);

            if (TimeoutSeconds.HasValue)
            {
                _client.WithTimeout(TimeoutSeconds.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken">"" login</param>
        private void AddHeaders(string accessToken)
        {
            _client.HttpClient.DefaultRequestHeaders.Clear();

            _client.WithHeader("Accept", "application/json");
            _client.WithHeader("User-Agent", "AbpApiClient");
            /* Disabled due to https://github.com/paulcbetts/ModernHttpClient/issues/198#issuecomment-181263988
               _client.WithHeader("Accept-Encoding", "gzip,deflate");
            */

            //if (_applicationContext.CurrentLanguage != null)
            //{
            //    _client.WithHeader(".AspNetCore.Culture", "c=" + _applicationContext.CurrentLanguage.Name + "|uic=" + _applicationContext.CurrentLanguage.Name);
            //}

            //if (_applicationContext.CurrentTenant != null)
            //{
            //    _client.WithHeader(_multiTenancyConfig.TenantIdResolveKey, _applicationContext.CurrentTenant.TenantId);
            //}

            if (!string.IsNullOrEmpty(accessToken))
            {
                _client.WithOAuthBearerToken(accessToken);
            }
        }

        public async Task<AbpAuthenticateResultModel> LoginAsync(AbpAuthenticateModel input)
        {
            
                var response = await GetClient("")
                    .Request(LoginUrlSegment)
                    .PostJsonAsync(input)
                    .ReceiveJson<AjaxResponse<AbpAuthenticateResultModel>>();

                if (!response.Success || response.Result == null)
                {
                    AuthenticateResult = null;
                    throw new Exception("");
                    //throw new UserFriendlyException(response.Error.Message + ": " + response.Error.Details);
                }
                AuthenticateResult = response.Result;
                AuthenticateResult.RefreshTokenExpireDate = DateTime.Now.Add(TimeSpan.FromDays(365));
                JwConsts.LastToken = AuthenticateResult.AccessToken;

                return AuthenticateResult;
            
        }

        /// <summary>
        /// 获取返回值 外面包一层 response的状态相关
        /// </summary>
        /// <returns></returns>
        public async Task<AjaxResponse<GetCurrentLoginInformationsOutput>> LoginWithLastToken()
        {
            return await GetAjaxResponse<GetCurrentLoginInformationsOutput>("api/services/app/Session/GetCurrentLoginInformations", null, AuthenticateResult.AccessToken);
            //GetCurrentLoginInformationsOutput robj =await GetAsync<GetCurrentLoginInformationsOutput>("api/services/app/Session/GetCurrentLoginInformations", null, AuthenticateResult.AccessToken, true);
            //return robj;
        }


        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
