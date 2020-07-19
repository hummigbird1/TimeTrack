# Configuration Blocks

See the default and example configuration files in addition to this documentation. 

## Time Track Manager Configuration
Json Element Name: `TimeTrackManagerConfiguration`

Used by: Console Application

Type: Anonymous type composed of the following properties

### Properties
* `AutoDiscardThreshold`
  
    Type: Timespan

    Default: null

    Description: Leaving this Value undefined or setting this value to 0 will disable the auto-discard feature

* `CaseSensitive`

    Type: bool

    Default: true

* `IgnoreRestartSameActivity`

    Type: bool

    Default: false

## Dependency Injection Service Selection

Json Element Name: `DependencyInjectionServiceSelection`

Used by: Console Application, Status UI Application

Type: Array of anonymous type composed of the following properties

### Properties
 
* `TypeName`

    Type: string

* `ServiceType`

    Type: string


## Aliases
Json Element Name: `Aliases`

Used by: Console Application

Type: Array of anonymous type composed of the following properties

### Properties
* `Name`
  
  Type: string

* `Arguments`
  
    Type: Array of string


## Application Settings
Json Element Name: `ApplicationSettings`

Used by: Status UI Application

Type: Anonymous type composed of the following properties

### Properties

* `DisableNotifications`
  
    Type: bool

    Default: false

* `UpdateInterval`
  
    Type: Timespan

    Default: 1 second


## Reminder Configuration
Json Element Name:  `ReminderConfiguration`

Used by: Status UI Application

Type: Anonymous type composed of the following properties

### Properties

* `SilentTimes`

    Type: Array of anonymous type composed of the following properties
    
  * `Start` 
  
    Type: Timespan

  * `End`
  
    Type: Timespan

  * `Days`
    
    Type: List of System.DayOfWeek

* `OnIdle` 
    * `DefinitionName`
    
        Type: string

        Description: The name of the toast notification definition to show when the idle reminder is triggered.

    * `NotificationThreshhold`
    
        Type: Timespan

        Default: 5 minutes

    * `RetriggerThreshhold`
    
        Type: Timespan

        Default: 5 Minutes

* `OnTest`
    
    Type: string

    Description: The name of the toast notification definition to show when testing the notifications.

* `OnTracking`

    Type: Array of anonymous type composed of the following properties


  * `Activities`
  
    Type: Array of string

  * `DefinitionName`
  
    Type: string
    
    Description: The name of the toast notification definition to show when the this tracking reminder is triggered.

  * `NotificationThreshhold`
  
    Type: Timespan

    Default: 5 minutes

  * `RetriggerThreshhold`
    
    Type: Timespan

    Default: 5 Minutes

* `ToastNotificationDefinitions` 

    Type: Array of anonymous type composed of the following properties

  * `DefinitionName`

    Type: string

    Description: The name of the definition (to be referred to by the `OnTest`, `OnIdle`, `OnTracking` definitions)

  * `ImageFileOrUrl`
  
    Type: string

    Description: Absolute or relative path to a file in the local file system, or a valid Url (beginning with http:// or https://)
    
    In case of a relative local file, the folder where the exe file is located is used as the base path (aka relative path is relative to the application file)


  * `LongDisplayDuration`
  
    Type: bool

    Default: false

    Description: The default (short) duration of a notification is approx. 7 seconds. When this property is set to true, the duration of notifications is approx. 25 seconds.

  * `LoopSound`

    Type: bool

    Default: false

    Description: When this property is set to true, the notification sound (if any is defined) will play looped for the duration while the notification is shown. 

  * `Sound`

    Type: string

    Valid Values: Default, IM, Mail, Reminder, SMS,
    Alarm, Alarm;1, Alarm;2, Alarm;3, Alarm;4, Alarm;5, Alarm;6, Alarm;7, Alarm;8, Alarm;9, Alarm;10, Call, Call;1, Call;2, Call;3, Call;4, Call;5, Call;6, Call;7, Call;8, Call;9, Call;10, none

  * `StandardTemplateName`
  
    Type: string

    Valid Values: ToastText01, ToastText02, ToastText03, ToastText04, ToastImageAndText01, ToastImageAndText02, ToastImageAndText03, ToastImageAndText04

    Additional Links: https://docs.microsoft.com/en-us/previous-versions/windows/apps/hh761494(v=win.10)

    https://docs.microsoft.com/en-us/uwp/api/windows.ui.notifications.toasttemplatetype?view=winrt-18362

  * `TextLines`
  
    Type: Array of string

    **Warning**: Do not specify more lines than the selected template provides.
    If no template is explicittly selected, the lines of text are used to determine the best template and therefore no template might be found of template can handle the amount of lines defined.  
		
#### Textlines Templating

The text lines allow for dynamic values to be used.
To use a dynamic placeholder enter the "property path" of the value you want to use in a block of double curly braces like {{ }}.

e.g.: "This line will habe the Identifier of the currently tracked identifier right here: {{ Data.CurrentTrackingActivity.Identifier }}" 

To see all available properties provided for use in templates see the following data structure.

##### Data Structure

* `Now`

    Type: DateTime

* `Data`

    * `CurrentTrackingActivity`

      * `Started`
        
        Type: DateTime

      * `Identifier`

        Type: string

      * `Created`

        Type: DateTime

      * `Modified`

        Type: DateTime

      * `RecordId`

        Type: string
   
    * `Duration`

        Type: Timespan

    * `IsTracking`

        Type: bool

    * `LastTrackedActivity`
        * `Identifier`

            Type: string

        * `Started`

            Type: DateTime

        * `Stopped`

            Type: DateTime

        * `Created`

            Type: DateTime

        * `Modified`

            Type: DateTime

        * `RecordId`

            Type: string

###### Formatting

To have the same formatting of timespans available that is used in the Status UI
you can use the special formatting functions provided:

* `ToOptimizedString`
    
    Resulting string will not have any milliseconds

    Parameters: bool

            true = Show no days in time span string (total hours e.g. 25 Hours => 25:00:00)
            false = Show days in time span string (e.g. 25 Hours => 1.01:00:00)
        

Use the function in the string template like shown in the following example

Example: 
`{{ Duration.ToOptimizedString(true) }}`