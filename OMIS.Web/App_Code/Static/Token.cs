using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using OMIS.Common;

/// <summary>
/// Token 的摘要说明
/// </summary>
public class Token
{
    protected static string TokenSession = "OMIS_TOKEN_SESSION_";
    protected static string TokenPrefix = "OMIS_TOKEN_";

    //最大超时时间，单位：分钟
    protected static int MaxMinutes = 45;

    public Token()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static string BuildToken()
    {
        string token = GetToken();
        return !token.Equals(string.Empty) ? String.Format("<input type=\"hidden\" id=\"txtFormToken\" value=\"{0}\" />", GetToken()) : string.Empty;
    }

    public static string GetToken()
    {
        try
        {
            if (!Config.ValidateRequestToken())
            {
                return string.Empty;
            }
            string token = BuildHttpRequestToken();
            token = UCAuthCode.AuthCodeEncode(token, BuildHttpRequestTotenKey());
            token = Base64Coding.EncodingForString(token);

            //ServerLog.WriteDebugLog(HttpContext.Current.Request, "Token", token);

            HttpContext.Current.Session[TokenSession] = token;

            return token;
        }
        catch (Exception ex)
        {
            ServerLog.WriteErrorLog(ex, HttpContext.Current);

            return string.Empty;
        }
    }

    public static string GetSaveToken()
    {
        if (HttpContext.Current.Session[TokenSession] != null)
        {
            return HttpContext.Current.Session[TokenSession].ToString();
        }
        return string.Empty;
    }

    public static bool CheckToken(string token)
    {
        return CheckToken(token, false);
    }

    public static bool CheckToken(string token, bool isDebug)
    {
        try
        {
            token = token.Replace(' ', '+');
            //客户端请求的TOKEN
            string requestToken = UCAuthCode.AuthCodeDecode(token, BuildHttpRequestTotenKey());
            
            //根据TOKEN生成规则生成的当前TOKEN
            string currentToken = GetSaveToken();
            currentToken = Base64Coding.DecodingForString(currentToken);
            currentToken = UCAuthCode.AuthCodeDecode(currentToken, BuildHttpRequestTotenKey());

            if (requestToken.Equals(currentToken))
            {
                string[] arrTokenValue = ParseTokenValue(requestToken);
                //判断SessionId 是否相同
                return arrTokenValue[1].Equals(BuildSessionId());
            }
            else
            {
                try
                {
                    string[] arrTokenValue = ParseTokenValue(requestToken);
                    string[] arrCurTokenValue = ParseTokenValue(currentToken);

                    DateTime dt = new DateTime(long.Parse(arrTokenValue[2]));
                    DateTime dtCur = new DateTime(long.Parse(arrCurTokenValue[2]));
                    TimeSpan ts = dtCur - dt;
                    //判断SessionId 是否相同 Token 时间超时 则无效
                    return arrTokenValue[1].Equals(BuildSessionId()) && ts.TotalMinutes < MaxMinutes;
                }
                catch (Exception ex)
                {
                    ServerLog.WriteErrorLog(ex, HttpContext.Current);

                    return false;
                }
            }
        }
        catch (Exception exx)
        {
            ServerLog.WriteErrorLog(exx, HttpContext.Current);

            return false;
        }
    }
    
    private static string BuildHttpRequestToken()
    {
        return String.Format("{0},{1},{2}", BuildHttpRequestTotenKey(), BuildSessionId(), DateTime.Now.Ticks.ToString());
    }

    private static string BuildHttpRequestTotenKey()
    {
        return Encrypt.HashEncrypt(TokenPrefix, EncryptFormat.ShortMd5);
    }

    private static string BuildSessionId()
    {
        return Encrypt.HashEncrypt(HttpContext.Current.Session.SessionID, EncryptFormat.ShortMd5);
    }

    private static string[] ParseTokenValue(string token)
    {
        return token.Split(',');
    }

}