#region namespace/imports
using NavegacionPorInstruccionesExternas.DTOs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
#endregion

namespace NavegacionPorInstruccionesExternas.Controllers
{
    public class SeleniumCommands
    {
        #region Propiedades y variables
        public ChromeDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }
        public IJavaScriptExecutor JSExecutor { get; set; }
        public string DownloadsTab { get; set; } = null;
        public OpenQA.Selenium.Interactions.Actions Action { get; set; }

        private readonly string UrlChromeDescargas = ConfigurationManager.AppSettings["UrlChromeDescargas"];
        private readonly string ChromeExe = ConfigurationManager.AppSettings["ChromeExe"];
        private readonly string ChromeDriver = ConfigurationManager.AppSettings["ChromeDriver"];

        private readonly int EsperaDescargaSeg     = Convert.ToInt32(ConfigurationManager.AppSettings["EsperaVentanaDescarga"], CultureInfo.CurrentCulture);
        private readonly bool VerNavegador         = Convert.ToBoolean(ConfigurationManager.AppSettings["VerNavegador"]);

        #endregion

        #region Constructor/configuracion Chrome
        public SeleniumCommands()
        {
            try
            {
                ChromeOptions CHOpt;
                ChromeDriverService CHDriverService;
                //string PathRoot = ConfigurationManager.AppSettings["DriverCH"].ToString(CultureInfo.CurrentCulture);
                string Chrome = Path.Combine("C:\\dobot\\VersionChrome\\chrome-win64", "chrome.exe").ToString(CultureInfo.CurrentCulture);

                if (File.Exists(Chrome) == true)
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Constructor" + "->" + "Existe el driver de Chrome");
                    CHOpt = new ChromeOptions { BinaryLocation = ChromeExe };
                    CHOpt.AddUserProfilePreference("download.default_directory", ConfigurationManager.AppSettings["CarpetaDescargas"]);
                    CHOpt.AddArgument("--start-maximized");
                    CHOpt.AddArgument("--disable-popup-bloking");
                    CHOpt.AddArgument("--disable-infobars");
                    CHOpt.AddArgument("--enable-automation");
                    CHOpt.AddArgument("--silent");
                    CHOpt.AddArgument("--log-level=3");
                    CHOpt.AddArgument("--no-sandbox");
                    if(!VerNavegador) CHOpt.AddArgument("--headless=new");
                    CHOpt.AcceptInsecureCertificates = true;
                    CHDriverService = ChromeDriverService.CreateDefaultService(ChromeDriver);
                    CHDriverService.HideCommandPromptWindow = true;
                    Driver = new ChromeDriver(CHDriverService, CHOpt);
                    JSExecutor = (IJavaScriptExecutor)Driver;
                    Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                    Action = new OpenQA.Selenium.Interactions.Actions(Driver);
                    Driver.Manage().Window.Maximize();

                }
            }
            catch (Exception ex)
            {
                Driver.Close();
                Driver.Quit();
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Constructor == Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
            }
        }

        #endregion

        #region Metodos Selenium

        public IWebElement FindElement(string locatorName, By locator)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(locator));
                IWebElement element = Driver.FindElement(locator);
                return element;
            }
            catch (WebDriverTimeoutException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.FindElement" + " : " + $"elemento {locatorName} WebDriverTimeoutException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
            catch (StaleElementReferenceException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.FindElement" + " : " + $"elemento {locatorName} StaleElementReferenceException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                IWebElement element = Driver.FindElement(locator);
                return element;
            }
            catch (NoSuchElementException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.FindElement" + " : " + $"elemento {locatorName} NoSuchElementException " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.FindElement" + " : " + $"elemento {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }

        }

        public bool Click(string locatorName, By locator)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(locator));
                Driver.FindElement(locator).Click();
                Thread.Sleep(1000);
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName}");
                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} WebDriverTimeoutException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
            catch (StaleElementReferenceException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} StaleElementReferenceException");
                if (Wait.Until(ExpectedConditions.ElementExists(locator)) != null)
                {

                    Driver.FindElement(locator).Click();
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} Exception superada");
                    return true;
                }
                else
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                    return false;
                }
            }
            catch (ElementNotInteractableException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} Exception:" + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;

            }
            catch (NoSuchElementException ex)
            {
                if (Wait.Until(ExpectedConditions.ElementExists(locator)) != null)
                {
                    Driver.FindElement(locator).Click();
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} Exception superada");
                    return true;
                }
                else
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} NoSuchElementException" + ex.Message.ToString(CultureInfo.CurrentCulture));
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("desechado"))
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Click en {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public string GetText(string locatorName, By locator)
        {
            try
            {
                string text = Driver.FindElement(locator).Text;
                return text;
            }
            catch (WebDriverTimeoutException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetText" + " : " + $"elemento {locatorName} WebDriverTimeoutException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
            catch (StaleElementReferenceException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetText" + " : " + $"elemento {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                Thread.Sleep(1000);
                string text = Driver.FindElement(locator).Text;
                if (text != null)
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetText" + " : " + $"elemento {locatorName} Exception superada");
                    return text;
                }
                else
                    return null;
            }
            catch (NoSuchElementException ex)
            {
                if (Wait.Until(ExpectedConditions.ElementExists(locator)) != null)
                {
                    string text = Driver.FindElement(locator).Text;
                    return text;
                }
                else
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetText" + " : " + $"elemento {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                    return null;
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetText" + " : " + $"elemento {locatorName} Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }

        public bool Type(string nombreCampo, By locator, string text)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(locator));
                Driver.FindElement(locator).SendKeys(text);
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Type" + " : " + $"Digitar texto en: {nombreCampo}");
                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Click" + " : " + $"Digitar texto en: {nombreCampo} WebDriverTimeoutException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Type" + " : " + "Digitar texto en: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public bool Clear(By locator)
        {
            try
            {
                Driver.FindElement(locator).Clear();
                return true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Clear" + " : " + "Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }

        }

        public bool Visit(string nombreLink, Uri url)
        {            
            try
            {
                Driver.Navigate().GoToUrl(url);
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Visit" + " : " + $"Visitar página {nombreLink} URL: {url}");
                
                return true;
            }
            catch (WebDriverTimeoutException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Visit" + " : " + $"Visitar página {nombreLink} WebDriverTimeoutException: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                
                return false;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.Visit == Visitar página Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public bool OpenNewTab()
        {
            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.open();");
                Driver.SwitchTo().Window(Driver.WindowHandles.Last());
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.OpenNewTab" + " : " + "Se abre nueva pestaña ");
                return true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.OpenNewTab" + " : " + "Exception: " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public bool ChangeToTab(string nombreVentana, string tabId)
        {
            try
            {
                bool switched = false;
                IWebDriver tab = null;
                foreach (string window in Driver.WindowHandles)
                {
                    if (window == tabId)
                    {
                        tab = Driver.SwitchTo().Window(window);
                        if (tab != null)
                        {
                            switched = true;
                            LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.ChangeToTab" + " : " + $"Se enfoca ventana {nombreVentana}");
                            break;
                        }
                    }
                }
                return switched;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.ChangeToTab == Exception : " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public Uri GetCurrentUrl()
        {
            Uri currentUrl;
            try
            {
                currentUrl = new Uri(Driver.Url);
                return currentUrl;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetCurrentUrl == Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }

        public string GetCurrentWindow()
        {
            try
            {
                string currentWindow;
                currentWindow = Driver.CurrentWindowHandle;
                return currentWindow;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetCurrentWindow == Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }             

        public bool ExecuteScript(IWebElement webElement, string script)
        {
            try
            {
                JSExecutor.ExecuteScript(script, webElement);
                return true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.ExecuteScript == Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public object SendScript(string nombreScript, string script)
        {
            object value;
            try
            {
                value = (object)JSExecutor.ExecuteScript("return " + script);
                
                return value;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.SendScript == Exception: Script "+ nombreScript + " " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }           

        public bool CloseTab()
        {
            try
            {
                Driver.Close();
                return true;
            }
            catch (Exception exception)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.CloseTab" + " : " + "Exception: " + exception.Message);
                return false;
            }
        }

        public bool IsDisplayed(string locatorName, By locator)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(locator));
                bool isDisPlayed = Driver.FindElement(locator).Displayed;
                return isDisPlayed;
            }
            catch (NoSuchElementException ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.IsDisplayed == Exception :" + $"elemento {locatorName} Exception: " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public bool WaitBrowserAllDownloadComplete(int numeroArchivosEsperados)
        {
            try
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserAllDownloadComplete" + " : " + "Inicia método: esperar descargas completas");
                int ciclosEsperados = 0;
                bool isDescargando = false;

                bool canContinueWaitAll = ChangeToTab("Descargas", DownloadsTab);
                if (canContinueWaitAll)
                {
                    canContinueWaitAll = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                }
                else
                {
                    canContinueWaitAll = OpenNewTab();
                    if (canContinueWaitAll)
                    {
                        canContinueWaitAll = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                        DownloadsTab = Driver.CurrentWindowHandle;
                    }
                }

                if (canContinueWaitAll)
                {   
                    do
                    {
                        object[] objElementosDescargados = GetDownloadedItems();
                       
                        isDescargando = false;

                        if (objElementosDescargados != null && objElementosDescargados.Any())
                        {
                            int numeroElementosDescargados = objElementosDescargados.Length;

                            if(numeroElementosDescargados == numeroArchivosEsperados)
                            {
                                for (int indexElementoDescargado = 0; indexElementoDescargado < numeroElementosDescargados; indexElementoDescargado++)
                                {
                                    Dictionary<string, object> propiedadesItemDescarga = GetPropertiesFromDownloadedItem(objElementosDescargados, indexElementoDescargado);

                                    if ((string)propiedadesItemDescarga["state"] == "IN_PROGRESS")
                                    {
                                        isDescargando = true;
                                    }
                                }
                            }
                            else
                            {
                                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserAllDownloadComplete" + " : " + $"Un numero diferente de archivos al esperado se encontraron en descargas {numeroElementosDescargados}");
                            }
                        }
                        
                        Thread.Sleep(1000);
                        ciclosEsperados++;

                    } while (isDescargando == true && ciclosEsperados < EsperaDescargaSeg);
                }

                if (isDescargando == true)
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserAllDownloadComplete" + " : " + $"Despues de {EsperaDescargaSeg} todavia hay descargas en progreso");
                }

                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserAllDownloadComplete" + " : " + "Finaliza método: esperar descargas completas " + $"ciclos esperados: {ciclosEsperados}");
                return !isDescargando;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserAllDownloadComplete == Exception : " + ex.Message.ToString(CultureInfo.CurrentCulture));
                return false;
            }
        }

        public ResultDownloadedFileDTO WaitBrowserDownloadComplete(string fileName)
        {
            try
            {
                ResultDownloadedFileDTO resultDownloadedFile = null;
                bool isIniciado = false;
                int ciclosEsperados = 0;
                bool isDescargado = false;
                string nombreArchivoDescargado = "";

                bool canContinueWait = ChangeToTab("Descargas", DownloadsTab);
                if (canContinueWait)
                {
                    canContinueWait = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                }
                else
                {
                    canContinueWait = OpenNewTab();
                    if (canContinueWait)
                    {
                        canContinueWait = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                        DownloadsTab = Driver.CurrentWindowHandle;
                    }
                }

                if (canContinueWait)
                {
                    do
                    {
                        object[] objElementosDescargados = GetDownloadedItems();

                        if (objElementosDescargados != null && objElementosDescargados.Any())
                        {
                            int numeroElementosDescargados = objElementosDescargados.Length;
                            for (int indexElementoDescargado = 0; indexElementoDescargado < numeroElementosDescargados; indexElementoDescargado++)
                            {
                                Dictionary<string, object> propiedadesItemDescarga = GetPropertiesFromDownloadedItem(objElementosDescargados, indexElementoDescargado);
                                string fileNameItemDescarga = (string)propiedadesItemDescarga["fileName"];
                                if (fileNameItemDescarga.Contains(fileName))
                                {

                                    isIniciado = true;

                                    if ((string)propiedadesItemDescarga["state"] == "IN_PROGRESS")
                                    {
                                        isIniciado = true;

                                    }
                                    if ((string)propiedadesItemDescarga["state"] == "COMPLETE")
                                    {

                                        nombreArchivoDescargado = fileNameItemDescarga;
                                        isDescargado = true;
                                        LOGRobotica.Controllers.LogApplication.LogWrite($"SeleniumCommands.WaitBrowserDownloadComplete : DESCARGA COMPLETA {nombreArchivoDescargado}");
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                        Thread.Sleep(1000);
                        ciclosEsperados++;

                    } while (isDescargado == false && ciclosEsperados < EsperaDescargaSeg);

                    if (isDescargado == true)
                    {
                        resultDownloadedFile = new ResultDownloadedFileDTO()
                        {
                            IsDownloadStarted = isIniciado,
                            IsDownloadCompleted = isDescargado,
                            FileName = nombreArchivoDescargado
                        };
                    }
                }
                return resultDownloadedFile;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.WaitBrowserDownloadComplete == Exception : " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }

        public ResultDownloadedFileDTO CheckingDowndloadStatus(string fileName)
        {
            try
            {
                bool encontrado = false;
                int esperas = 0;
                bool isDescargado = false;
                ResultDownloadedFileDTO result = new ResultDownloadedFileDTO();

                bool canContinue = ChangeToTab("Descargas", DownloadsTab);
                if (canContinue)
                {
                    canContinue = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                }
                else
                {
                    canContinue = OpenNewTab();

                    if (canContinue)
                    {
                        canContinue = Visit(nameof(UrlChromeDescargas), new Uri(UrlChromeDescargas));
                        DownloadsTab = Driver.CurrentWindowHandle;
                    }
                }

                if (canContinue)
                {
                    do
                    {
                        Thread.Sleep(1000);
                        object[] objElementosDescargadosCheck = GetDownloadedItems();
                        if (objElementosDescargadosCheck != null && objElementosDescargadosCheck.Any())
                        {
                            int numeroElementosDescargados = objElementosDescargadosCheck.Length;
                            for (int indexElementoDescargado = 0; indexElementoDescargado < numeroElementosDescargados; indexElementoDescargado++)
                            {
                                Dictionary<string, object> propiedadesItemDescargaCheck = GetPropertiesFromDownloadedItem(objElementosDescargadosCheck, indexElementoDescargado);
                                string fileNameItemDescarga = (string)propiedadesItemDescargaCheck["fileName"];
                                if (fileNameItemDescarga.Contains(fileName))
                                {
                                    result = new ResultDownloadedFileDTO
                                    {
                                        FileName = fileNameItemDescarga,
                                        IsDownloadStarted = true
                                    };
                                    encontrado = true;
                                   

                                    if ((string)propiedadesItemDescargaCheck["state"] == "IN_PROGRESS")
                                    {
                                        isDescargado = false;
                                    }
                                    if ((string)propiedadesItemDescargaCheck["state"] == "COMPLETE")
                                    {
                                        isDescargado = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                        esperas++;
                    } while (encontrado == false && esperas < EsperaDescargaSeg);
                }
                result.IsDownloadCompleted = isDescargado;
                return result;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.CheckingDowndloadStatus == Exception : " +  ex.Message.ToString(CultureInfo.CurrentCulture));

                return null;
            }
        }

        public object[] GetDownloadedItems()
        {
            try
            {
                object objDescargas = ((IJavaScriptExecutor)Driver).ExecuteScript("return document.querySelector('downloads-manager').shadowRoot.getElementById('downloadsList').items;");
                object[] listaDescarga = ((IEnumerable<object>)objDescargas).ToArray();
                return listaDescarga;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetDownloadedItems == Exception : " +  ex.Message.ToString(CultureInfo.CurrentCulture));
                return null;
            }
        }

        private Dictionary<string, object> GetPropertiesFromDownloadedItem(object[] objElementosDescargados, int indexDescarga)
        {
            try
            {
                object itemDescarga = objElementosDescargados[indexDescarga];
                Dictionary<string, object> propiedadesItemDescarga = ((Dictionary<string, object>)itemDescarga);
                return propiedadesItemDescarga;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("SeleniumCommands.GetPropertiesFromDownloadedItem == Exception : " +  ex.Message.ToString(CultureInfo.CurrentCulture));

                return null;
            }
        }
        #endregion
    }
}