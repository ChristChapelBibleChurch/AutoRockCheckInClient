#region Using statements

using System;
using System.Collections.Generic;
using System.Diagnostics;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

#endregion

namespace org.christchapelbc.RockRMS.CheckIn.Web.AutomatedClient
{
    /// <summary>
    ///     The main class of this application.
    /// </summary>
    public sealed class Program
    {
        /// <summary>
        ///     The entry point of this application.
        /// </summary>
        public static void Main()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            string userName = "admin";
            string password = @"admin";

            string themeName    = "checkinpark";
            int kioskId         = 2;

            using ( IWebDriver driver = new ChromeDriver() )
            {
                // Start up the web browser
                driver.Navigate().GoToUrl( new Uri( "https://rock.rocksolidchurchdemo.com/checkin" ) );

                #region Log in

                // Log in
                driver.SendKeysToElement( By.XPath( "//input[contains(@id, 'tbUserName') and @type='text']" ), userName );
                driver.SendKeysToElement( By.XPath( "//input[contains(@id, 'tbPassword') and @type='password']" ), password );
                driver.ClickElement( By.XPath( "//a[contains(@id, 'btnLogin')]" ) );
                driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

                #endregion

                #region Configure kiosk

                // Configure kiosk
                driver.ClickElement( By.XPath( $@"//select[contains(@id, 'ddlTheme')]/option[@value='{ themeName }']" ) );
                driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

                driver.ClickElement( By.XPath( $@"//select[contains(@id, 'ddlKiosk')]/option[@value='{ kioskId }']" ) );
                driver.WaitForAjax( TimeSpan.FromSeconds( 5 ) );

                // Check-in Configuration is automatically set to the default

                driver.WaitForElement( By.XPath( "//div[contains(@id, 'pnlManualConfig')]//div[contains(@class, 'rock-check-box-list')]" ), TimeSpan.FromSeconds( 5 ) );

                ISet<int> checkInAreaIds = new HashSet<int>()
                {
                    18, // Check-in Test Area
                    19, // Nursery/Preschool Area
                    20, // Elementary Area
                    21, // Jr High Area
                    22, // High School Area
                };
                foreach ( int checkInAreaId in checkInAreaIds )
                {
                    driver.ClickElement( By.XPath( $"//input[contains(@id, 'cblPrimaryGroupTypes') and @type='checkbox' and @value='{ checkInAreaId }']" ) );
                }

                ISet<int> additionalAreaIds = new HashSet<int>()
                {
                    23, // Serving Team
                };
                foreach ( int additionalAreaId in additionalAreaIds )
                {
                    driver.ClickElement( By.XPath( $"//input[contains(@id, 'cblAlternateGroupTypes') and @type='checkbox' and @value='{ additionalAreaId }']" ) );
                }

                driver.ClickElement( By.XPath( $"//a[contains(@id, 'lbOk')]" ) );
                driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

                #endregion

                #region Check in people

                // Check in people
                driver.CheckInPerson( "3322", 68, 6, 6, null, null, null, "Bears Room" );
                driver.CheckInPerson( "3322", 68, 7, 6, null, null, null, "Bunnies Room" );

                #endregion

                // Shut down the web browser
                driver.Quit();
            }

            timer.Stop();

            Console.WriteLine
                (
                    String.Format
                        (
                            "Finished in {0:00}:{1:00}:{2:00}.{3:00}",
                            timer.Elapsed.Hours,
                            timer.Elapsed.Minutes,
                            timer.Elapsed.Seconds,
                            ( timer.Elapsed.Milliseconds / 10 )
                        )
                )
            ;
        }
    }

    static class Extensions
    {
        public static void CheckInPerson
            (
                this IWebDriver driver,
                string phoneNumber,
                int familyId,
                int personId,
                int? serviceTimeId      = null,
                string? areaName        = null,
                string? groupName       = null,
                string? abilityLevel    = null,
                string? locationName    = null
            )
        {
            // Validate parameters
            if ( driver == null )
            {
                throw new ArgumentNullException( nameof( driver ) );
            }

            if ( String.IsNullOrWhiteSpace( phoneNumber ) )
            {
                throw new ArgumentNullException( nameof( phoneNumber ) );
            }

            if ( ( phoneNumber.Length != 4 && phoneNumber.Length != 10 ) || !"^[0-9]+$".Matches( phoneNumber ) )
            {
                Console.WriteLine( $"ERR: '{ phoneNumber }' is not a valid phone number." );

                return;
            }

            // Start check-in process
            driver.ClickElement( By.XPath( "//a[contains(@class, 'btn-checkin')]" ) );
            driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

            // Search by phone number
            driver.SendKeysToElement( By.XPath( "//input[contains(@id, 'tbPhone') and @type='text']" ), phoneNumber );
            driver.ClickElement( By.XPath( "//a[contains(@id, 'lbSearch')]" ) );
            driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

            // Select family
            driver.ClickElement( By.XPath( $"//div[contains(@id, 'pnlSelectFamilyPostback') and contains(@data-target, '{ familyId }')]" ) );
            driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

            // Select family member
            By xpath = By.XPath( $"//div[contains(@id, 'pnlPersonButton')]/a[@data-person-id='{ personId }']" );

            if ( driver.ElementExists( xpath ) )
            {
                if ( !driver.FindElement( xpath ).GetAttribute( "class" ).Contains( "active", StringComparison.OrdinalIgnoreCase ) )
                {
                    driver.ClickElement( xpath );
                }
            }

            driver.ClickElement( By.XPath( "//a[contains(@id, 'lbSelect')]" ) );
            driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );

            // Select service time
            if ( serviceTimeId.HasValue )
            {
                xpath = By.XPath( $"//button[@schedule-id='{ serviceTimeId }']" );

                if ( driver.ElementExists( xpath ) )
                {
                    driver.ClickElement( xpath );
                    driver.ClickElement( By.XPath( "//a[contains(@id, 'lbSelect')]" ) );
                    driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
                }
            }

            // Select area
            if ( !String.IsNullOrWhiteSpace( areaName ) )
            {
                xpath = By.XPath( $"//a[text()[contains(., \"{ areaName }\")]]" );

                if ( driver.ElementExists( xpath ) )
                {
                    driver.ClickElement( xpath );
                    driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
                }
            }

            // Select group
            if ( !String.IsNullOrWhiteSpace( groupName ) )
            {
                xpath = By.XPath( $"//a[text()[contains(., \"{ groupName }\")]]" );

                if ( driver.ElementExists( xpath ) )
                {
                    driver.ClickElement( xpath );
                    driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
                }
            }

            // Select ability level
            if ( !String.IsNullOrWhiteSpace( abilityLevel ) )
            {
                xpath = By.XPath( $"//a[text()[contains(., \"{ abilityLevel }\")]]" );

                if ( driver.ElementExists( xpath ) )
                {
                    driver.ClickElement( xpath );
                    driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
                }
            }

            // Select location
            if ( !String.IsNullOrWhiteSpace( locationName ) )
            {
                xpath = By.XPath( $"//a[text()[contains(., \"{ locationName }\")]]" );

                if ( driver.ElementExists( xpath ) )
                {
                    driver.ClickElement( xpath );
                    driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
                }
            }

            // Finish check-in process
            xpath = By.XPath( "//a[contains(@id, 'lbDone')]" );

            if ( driver.ElementExists( xpath ) )
            {
                driver.ClickElement( xpath );
                driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
            }
            else
            {
                Console.WriteLine( $"ERR: Unable to check { familyId }:{ personId } in to { groupName }:{ locationName }" );

                driver.ClickElement( By.XPath( "//a[contains(@id, 'lbCancel')]" ) );
                driver.WaitForPostBack( TimeSpan.FromSeconds( 5 ) );
            }
        }
    }
}
