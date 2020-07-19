import subprocess

class ConsoleExecuter(object):

    _consoleExecutablePath = ''
    _configurationPath = None
    _debug_enabled = False

    def setDebug(self, enable):
        self._debug_enabled = enable

    
    CREATE_NO_WINDOW = 0x08000000

    def __init__(self, consoleExecutablePath, configurationPath):
        self._consoleExecutablePath = consoleExecutablePath
        self._configurationPath = configurationPath

    def _create_default(self, verb):
        args = [ self._consoleExecutablePath, verb ]
        if self._configurationPath != None and self._configurationPath != '':
            args.extend(['--config', self._configurationPath])

        return args

    def _execute_only(self, args):
        if self._debug_enabled == True:
            print('Execution arguments:')
            print(args)

        result = subprocess.run(args, creationflags=self.CREATE_NO_WINDOW)

        if self._debug_enabled == True:
            print("EXITCODE: " + str(result.returncode))

        return result.returncode == 0

    def _execute_with_window(self, args):
        if self._debug_enabled == True:
            print('Execution arguments:')
            print(args)

        result = subprocess.run(args)

        if self._debug_enabled == True:
            print("EXITCODE: " + str(result.returncode))

        return result.returncode == 0

    def _execute_with_output(self, args):
        if self._debug_enabled == True:
            print('Execution arguments:')
            print(args)

        result = subprocess.Popen(args, creationflags=self.CREATE_NO_WINDOW, stdout=subprocess.PIPE, universal_newlines=True,encoding='utf-8')
        output_lines=[]
        while True:
            line = result.stdout.readline()
            if self._debug_enabled == True:
                print("LINE RECEIVED: " + line)
            if not line:
                break

            output_lines.append(line.rstrip())

        result.wait()
        if self._debug_enabled == True:
            print("EXITCODE: " + str(result.returncode))
            print("STDOUT:" + "\r\n".join(map(str, output_lines)))

            
        return result.returncode == 0, output_lines 


    def start(self, identifier):
        baseArgs = self._create_default('start')
        if identifier:
            baseArgs.extend(['-i', identifier])
        return self._execute_only(baseArgs)

    def stop(self, discard):
        baseArgs = self._create_default('stop')
        if discard == True:
            baseArgs.extend(['-d'])
        
        return self._execute_only(baseArgs)

    def query(self, informationType, fromDate):
        baseArgs = self._create_default('query')
        baseArgs.extend(['-t', informationType])

        if fromDate != None:
            baseArgs.extend(['--from', fromDate])

        return self._execute_with_output(baseArgs)

    def queryCurrentlyActive(self):
        result = self.query('p_idle_since', None)
        if result[0] == True:
            return TrackingActivityResult(result[1][0])

    def queryLastIdentifiers(self):
        result = self.query('lastidentifiers', 'Yesterday')
        if result[0] == True:
            return result[1]

    def queryAvailableAliases(self):
        result = self.query('available-aliases', None)
        if result[0] == True:
            return result[1]

    def runAlias(self, aliasName):
        baseArgs = self._create_default('run')
        baseArgs.extend(['-a', aliasName])
        baseArgs.extend(['--confirm'])
        return self._execute_with_window(baseArgs)

    def runCustomCommand(self, custom_command):
        baseArgs = self._create_default(custom_command.Verb)
        baseArgs.extend(custom_command.Arguments)
        if custom_command.Show_Window:
            return self._execute_with_window(baseArgs)
        else:
            return self._execute_only(baseArgs)

class TrackingActivityResult:
    idleDurationSeconds = 0
    timespan = ""
    eventTime = ""
    activity = ""
    def __init__(self, consoleOutputString):
        outputList = consoleOutputString.split('|')
        self.idleDurationSeconds = int(outputList[0])
        self.timespan = str(outputList[1])
        self.eventTime = str(outputList[2])
        self.activity = str(outputList[3])

    def isIdle(self):
        return self.idleDurationSeconds != 0
