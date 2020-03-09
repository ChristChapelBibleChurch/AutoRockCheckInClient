#region Using statements

using System;
using System.Collections.Generic;
using System.Threading;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

#endregion

namespace org.christchapelbc.RockRMS.CheckIn.Web.AutomatedClient
{
    /// <summary>
    ///     A collection of methods that extend the functionality
    ///     of the <see cref="IWebDriver" /> interface.
    /// </summary>
    internal static class IWebDriverExtensions
    {
        private static IWebElement? _page;

        public static bool ClickElement( this IWebDriver driver, By by )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to click the element
            if ( IWebDriverExtensions.ElementExists( driver, by ) )
            {
                driver.FindElement( by ).MoveToAndClick( driver );
                driver.WaitFor( TimeSpan.FromSeconds( 0.25 ) );

                return true;
            }

            return false;
        }

        public static bool ClickElement( this IWebDriver driver, By by, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to click the element
            if ( IWebDriverExtensions.ElementExists( driver, by, timeout ) )
            {
                driver.FindElement( by ).MoveToAndClick( driver );
                driver.WaitFor( TimeSpan.FromSeconds( 0.25 ) );

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines whether a given HTML element exists on the web page.
        /// </summary>
        /// 
        /// <param name="driver">
        ///     The service that will interact with the web browser.
        /// </param>
        /// 
        /// <param name="by">
        ///     The locating mechanism to use.
        /// </param>
        /// 
        /// <returns>
        ///     <c>true</c> if the given HTML element exists on the web
        ///     page; otherwise <c>false</c>.
        /// </returns>
        public static bool ElementExists( this IWebDriver driver, By by )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to find the element
            try
            {
                driver.FindElement( by );

                // The element was found
                return true;
            }
            catch ( NoSuchElementException )
            {
                // The element was not found
            }

            return false;
        }

        public static bool ElementExists( this IWebDriver driver, By by, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to wait until the element is found
            if ( timeout.TotalMilliseconds > 0 )
            {
                WebDriverWait wait = new WebDriverWait( driver, timeout );

                return wait.Until( x => IWebDriverExtensions.ElementExists( x, by ) );
            }

            return IWebDriverExtensions.ElementExists( driver, by );
        }

        public static IWebElement FindElement( this IWebDriver driver, By by, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to wait until the element is found
            if ( timeout.TotalMilliseconds > 0 )
            {
                WebDriverWait wait = new WebDriverWait( driver, timeout );

                return wait.Until( x => x.FindElement( by ) );
            }
            else
            {
                return driver.FindElement( by );
            }
        }

        public static IReadOnlyCollection<IWebElement> FindElements( this IWebDriver driver, By by, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to wait until the element is found
            if ( timeout.TotalMilliseconds > 0 )
            {
                WebDriverWait wait = new WebDriverWait( driver, timeout );

                return wait.Until( x => x.FindElements( by ) );
            }
            else
            {
                return driver.FindElements( by );
            }
        }

        public static bool SendKeysToElement( this IWebDriver driver, By by, string text )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            if ( String.IsNullOrEmpty( text ) )
            {
                throw new ArgumentNullException( nameof( text ) );
            }

            // Attempt to type in the element
            if ( IWebDriverExtensions.ElementExists( driver, by ) )
            {
                driver.FindElement( by ).MoveToAndSendKeys( driver, text );

                return true;
            }

            return false;
        }

        public static bool SendKeysToElement( this IWebDriver driver, By by, string text, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            if ( String.IsNullOrEmpty( text ) )
            {
                throw new ArgumentNullException( nameof( text ) );
            }

            // Attempt to type in the element
            if ( IWebDriverExtensions.ElementExists( driver, by, timeout ) )
            {
                driver.FindElement( by ).MoveToAndSendKeys( driver, text );

                return true;
            }

            return false;
        }

        public static void WaitFor( this IWebDriver driver, TimeSpan timeSpan )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            // Attempt to wait
            if ( timeSpan.TotalMilliseconds > 0 )
            {
                Thread.Sleep( (int)Math.Ceiling( timeSpan.TotalMilliseconds ) );
            }
        }

        public static void WaitForAjax( this IWebDriver driver, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            // Attempt to wait until the document is ready
            WebDriverWait documentWait = new WebDriverWait( driver, timeout );
            documentWait.Until( x => ( (IJavaScriptExecutor)x ).ExecuteScript( "return $.active == 0;" ) );
        }

        public static void WaitForElement( this IWebDriver driver, By by, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( by == null )
            {
                throw new ArgumentNullException( nameof( by ) );
            }

            // Attempt to wait for the element
            WebDriverWait wait = new WebDriverWait( driver, timeout );
            wait.Until( x => IWebDriverExtensions.ElementExists( x, by ) );
        }

        public static void WaitForPostBack( this IWebDriver driver, TimeSpan timeout )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            // Attempt to wait for the current page to stale
            if ( IWebDriverExtensions._page != null )
            {
                WebDriverWait pageWait = new WebDriverWait( driver, timeout );
                pageWait.Until( SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf( IWebDriverExtensions._page ) );
            }

            // Attempt to wait until the document is ready
            WebDriverWait documentWait = new WebDriverWait( driver, timeout );
            documentWait.Until( x => ( (IJavaScriptExecutor)x ).ExecuteScript( "return document.readyState;" ).Equals( "complete" ) );

            // Store the reference to the current page
            IWebDriverExtensions._page = driver.FindElement( By.TagName( "html" ) );
        }
    }
}
