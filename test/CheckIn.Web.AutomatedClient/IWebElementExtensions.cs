#region Using statements

using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

#endregion

namespace org.christchapelbc.RockRMS.CheckIn.Web.AutomatedClient
{
    internal static class IWebElementExtensions
    {
        public static void MoveToAndClick( this IWebElement element, IWebDriver driver )
        {
            // Validate parameters
            if ( element == null )
            {
                throw new ArgumentNullException( nameof( element ) );
            }

            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            try
            {
                element.Click();
            }
            catch ( ElementNotInteractableException )
            {
                // If the element isn't on the screen,
                // move and try again
                Actions actions = new Actions( driver );

                actions.MoveToElement( element );
                actions.Click();

                actions.Perform();
            }
        }

        public static void MoveToAndSendKeys( this IWebElement element, IWebDriver driver, string text )
        {
            // Validate parameters
            if ( element == null )
            {
                throw new ArgumentNullException( nameof( element ) );
            }

            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( String.IsNullOrEmpty( text ) )
            {
                throw new ArgumentNullException( nameof( text ) );
            }

            try
            {
                element.SendKeys( text );
            }
            catch ( ElementNotInteractableException )
            {
                // If the element isn't on the screen,
                // move and try again
                Actions actions = new Actions( driver );

                actions.MoveToElement( element );
                element.SendKeys( text );

                actions.Perform();
            }
        }
    }
}
