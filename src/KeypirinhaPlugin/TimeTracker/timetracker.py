import keypirinha as kp
import keypirinha_util as kpu

import datetime
import os
import subprocess
import time

from .lib import ConsoleAppWrapper
from .lib import CustomConsoleCommand

class TimeTracker(kp.Plugin):

    DEFAULT_ITEM_LABEL = "TimeTracker:"
    _executable_path = ""
    _configuration_path = ""
    ACTION_CATEGORY = kp.ItemCategory.USER_BASE + 1
    ALIAS_CATEGORY = kp.ItemCategory.USER_BASE + 2
    QUERY_RESULT_CATEGORY = kp.ItemCategory.USER_BASE + 255
    
    aliases_as_catalogitems = False
    query_identifiers = False
    history = []
    permanent_identifiers = []
    aliases = []

    custom_console_commands = []

    def __init__(self):
        super().__init__()
        self._item_label = self.DEFAULT_ITEM_LABEL

    def on_events(self, flags):
        """Reloads the package config when its changed
        """
        if flags & kp.Events.PACKCONFIG:
            self._read_config()
            self.on_catalog()

    def on_start(self):
        self._read_config()

        self.set_actions(self.ACTION_CATEGORY, [
            self._create_clearhistory_action()
           ])

    def on_catalog(self):
        if self._executable_path == '':
            self.info("The executable path for the console program is not configured. Plugin is not ready for use!")
            self._on_catalog_notconfigured()
        else:
            self._on_catalog_core()

    def on_suggest(self, user_input, items_chain):
        if not items_chain or items_chain[-1].category() != kp.ItemCategory.KEYWORD:
            return

        originalItem = items_chain[-1]

        suggestions = []

        if originalItem.target() == 'query':
            suggestions.extend(self._on_suggest_query())
            

        if originalItem.target() == 'start':
            suggestions.extend(self._on_suggest_start(originalItem, user_input))

        self.set_suggestions(suggestions, kp.Match.DEFAULT, kp.Sort.DEFAULT)

    def on_execute(self, item, action):
        self.dbg("Execute selected item: " + item.target())

        if item.target() == "custom_actions":
            self._execute_custom_actions(action)
            return

        if item.target() == "alias":
            self._execute_alias_acion(action)
            return

        if item.target() == "QUERY-RESULT":
            self.dbg('Query action is nothing to be executed! This is fine!')
            return

        parts = item.target().split(';')
        command = parts[0]

        arg = None
        if len(parts) > 1:
            arg = parts[1]

        executor = ConsoleAppWrapper.ConsoleExecuter(self._executable_path, self._configuration_path)
        executor.setDebug(self._debug)

        if command == "start":
            result = executor.start(arg)
            if result == True and arg != '':
                self.dbg('Adding to history:' + arg)
                self.history.append(arg)
            else:
                self.err('Start command failed!')
        
        if command == "custom_console_command":
            self._execute_custom_console_command(arg, executor)

        if command == "alias":
            executor.runAlias(arg)

        if command == "stop":
            executor.stop(False)
        
        if command == "discard":
            executor.stop(True)

    def _execute_custom_console_command(self, name, console_executor):
        for command in self.custom_console_commands:
            if command.Name == name:
                self.dbg("Executing console custom command {} (Verb: {} Arguments: {} Window-Shown: {})".format(command.Name, command.Verb, command.Arguments, command.Show_Window))
                console_executor.runCustomCommand(command)
                break

    def _on_catalog_notconfigured(self):
        self.dbg('Executable path empty, adding error item to catalog')
        self.set_catalog([
            self.create_item(
        category=kp.ItemCategory.KEYWORD,
        label="{} {}".format(self._item_label, "ERROR"),
        short_desc="Plugin not configured! Please configure this plugin and try again ...",
        target="error",
        args_hint=kp.ItemArgsHint.FORBIDDEN,
        hit_hint=kp.ItemHitHint.IGNORE)
        ])

    def _create_alias_action_items(self, aliases):
            aliasActions = []
            counter = 0
            for alias in self.aliases:
                counter = counter + 1
                aliasActions.append(self.create_action(
                    name="alias_{}".format(counter),
                    data_bag = alias,
                    label="{}".format(alias),
                    short_desc="Run {}".format(alias))
                )
            return aliasActions

    def _on_catalog_set_catalog_items(self):
            self.dbg('Adding catalog items') 
            catalogItems = [
                self._create_start_item(),
                self._create_stop_item(),
                self._create_discard_item(),
                self._create_action_item(),
                self._create_query_item(),
                self._create_alias_action(),
                ]


            self.dbg('Adding custom console commands catalog items') 
            for customcommand in self.custom_console_commands:
                catalogItems.append(self._create_custom_console_command_item(customcommand.Name, customcommand.Description))

            if self.aliases_as_catalogitems:
                self.dbg('Adding console aliases catalog items') 
                for alias in self.aliases:
                    catalogItems.append(self._create_alias_item(alias))

            self.set_catalog(catalogItems)
            self.info("{} catalog items added".format(len(catalogItems))) 

    def _on_catalog_core(self):
            start_time = time.time()
            self.dbg('Initiating console executer')
            executor = ConsoleAppWrapper.ConsoleExecuter(self._executable_path, self._configuration_path)
            executor.setDebug(self._debug)
            
            self.dbg('Loading aliases')
            self.aliases = executor.queryAvailableAliases()

            self._on_catalog_set_catalog_items()

            self.dbg('Setting alias action items')
            self.set_actions(self.ALIAS_CATEGORY, self._create_alias_action_items(self.aliases))

            self.history = []
            if self.query_identifiers == True:
                self.dbg('Load last identifiers')
                identifiers = executor.queryLastIdentifiers()

                if identifiers != None:
                    self.history.extend(map(str,identifiers))
            
            elapsed = time.time() - start_time
            self.info("Cataloged items in {:0.1f} seconds".format(elapsed))



    def _on_suggest_query(self):
        self.dbg('Load current running')
        executor = ConsoleAppWrapper.ConsoleExecuter(self._executable_path, self._configuration_path)
        executor.setDebug(self._debug)
        tar = executor.queryCurrentlyActive()
        self.dbg(tar)
        suggestions = []
        if not tar.isIdle():
            suggestions.append( self._create_query_suggestion(
                "Currently Tracking: " + tar.activity,
                "Duration: " + tar.timespan + " Started: " + tar.eventTime
            ))
        else:
            suggestions.append(self._create_query_suggestion(
                "Currently Tracking: <Nothing> | Idle for " + tar.timespan,
                "No activity is being tracked since '" +  tar.activity + "' stopped at " + tar.eventTime
            ))
        return suggestions

    def _on_suggest_start(self, originalItem, user_input):
        suggestions = []
        self.dbg('Creating suggestions for start action')
        if user_input:
            suggestions.append(self._create_start_suggestion(originalItem, user_input))

        for h in self.history:
            suggestions.append(self._create_start_suggestion(originalItem, h))
        
        if self.permanent_identifiers != None:
            for p in self.permanent_identifiers:
                suggestions.append(self._create_start_suggestion(originalItem, p.strip()))

        return suggestions

    
    def _create_query_suggestion(self, label, description):
        return self.create_item(
            category=self.QUERY_RESULT_CATEGORY,
            label=label,
            short_desc=description,
            target="QUERY-RESULT",
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _execute_custom_actions(self, action):
        self.dbg("Executing custom action")
        self.dbg(action.name())
        if action.name() == "clear_history":
            self.history = []

    def _execute_alias_acion(self, action):
        self.dbg("Executing alias")
        self.dbg(action.name())
        executor = ConsoleAppWrapper.ConsoleExecuter(self._executable_path, self._configuration_path)
        executor.setDebug(self._debug)
        executor.runAlias(action.data_bag())


    def _read_config(self):
        """Reads the default action from the config
        """
        self.dbg("Reading config")
        settings = self.load_settings()


        self._debug = settings.get_bool("debug", "main", False)

        self._item_label = settings.get("item_label", "main", self.DEFAULT_ITEM_LABEL)
        self.dbg("item_label =", self._item_label)

        self._executable_path = settings.get("executable_path", "console", "")
        self.dbg("executable_path =", self._executable_path)
        self._configuration_path = settings.get("configuration_path", "console", "")
        self.dbg("configuration_path =", self._configuration_path)

        self.permanent_identifiers = settings.get("permanent_identifiers", "start_suggestions", "").split(',') 
        self.dbg("permanent_identifiers =", self.permanent_identifiers)   

        self.query_identifiers = settings.get_bool("query_identifiers", "start_suggestions", False) 
        self.dbg("query_identifiers =", self.query_identifiers)   

        self.aliases_as_catalogitems = settings.get_bool("alias_as_catalogitems", "alias", False) 
        self.dbg("aliases_as_catalogitems =", self.aliases_as_catalogitems)

        self.custom_console_commands = []
        for section in settings.sections():
            if str.startswith(section, "console_custom_command/"):
                name = settings.get("label", section, "")
                if name != "":
                    command = self._read_custom_console_command(settings, section, name)
                    self.custom_console_commands.append(command)
                    self.dbg("Read console custom command '{}' from config (Verb: {} => Arguments: {} => Window-Shown: {})".format(command.Name, command.Verb, command.Arguments, command.Show_Window))

        self.dbg("{} custom commands defined".format(len(self.custom_console_commands)))
        self.dbg("Configuration set")

    def _read_custom_console_command(self, settings, section, name):
        command = CustomConsoleCommand.CustomConsoleCommand(name)
        command.Description = settings.get("description", section, "Run custom command {}".format(name))
        command.Verb = settings.get("verb", section, "")
        command.Show_Window = settings.get_bool("show_window", section, False)
        
        args = []
        for arg in settings.get("arguments", section, "").split('|'):
            strippedarg = str.strip(arg)
            if strippedarg != "":
                args.append(strippedarg)

        if not command.Show_Window:
            for a in args:
                if a == "--confirm":
                    self.warn("Overriding show_window for console custom command '{}' (Section {}). The 'confirm' argument was specified!".format(name, section))
                    command.Show_Window = True
                    break

        command.Arguments = args
        
        return command



    def _create_start_suggestion(self, originalItem, identifier):
        return self.create_item(
                category=originalItem.category(),
                label="Start tracking " + identifier,
                short_desc="Start tracking time for {} and stop any currently active tracked".format(identifier),
                target="{};{}".format(originalItem.target(), identifier),
                args_hint=kp.ItemArgsHint.FORBIDDEN,
                hit_hint=kp.ItemHitHint.NOARGS)

    def _create_alias_action(self):
        return self.create_item(
            category=self.ALIAS_CATEGORY,
            label="{} {}".format(self._item_label, "Run alias"),
            short_desc="Run any of {} defined aliases".format(len(self.aliases)),
            target="alias",
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_custom_console_command_item(self, name, description):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, name),
            short_desc=description,
            target="custom_console_command;{}".format(name),
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_alias_item(self, aliasName):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, aliasName),
            short_desc="Run alias {}".format(aliasName),
            target="alias;{}".format(aliasName),
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_start_item(self):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, "Start"),
            short_desc="Start tracking an activity and stop a running one",
            target="start",
            args_hint=kp.ItemArgsHint.ACCEPTED,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_stop_item(self):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, "Stop"),
            short_desc="Stop tracking an activity",
            target="stop",
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_discard_item(self):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, "Discard"),
            short_desc="Stop tracking an activity and discard",
            target="discard",
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_query_item(self):
        return self.create_item(
            category=kp.ItemCategory.KEYWORD,
            label="{} {}".format(self._item_label, "Query"),
            short_desc="Get currently tracking",
            target="query",
            args_hint=kp.ItemArgsHint.REQUIRED,
            hit_hint=kp.ItemHitHint.NOARGS)


    def _create_action_item(self):
        return self.create_item(
            category=self.ACTION_CATEGORY,
            label="{} {}".format(self._item_label, "Actions"),
            short_desc="Actions",
            target="custom_actions",
            args_hint=kp.ItemArgsHint.FORBIDDEN,
            hit_hint=kp.ItemHitHint.NOARGS)

    def _create_clearhistory_action(self):
         return self.create_action(
                name="clear_history",
                label="Clear History",
                short_desc="Clears history of entries")



