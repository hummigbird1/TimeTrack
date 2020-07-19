class CustomConsoleCommand(object):
    Name = ""
    Description = ""
    Verb = ""
    Arguments = []
    Show_Window = False

    def __init__(self, name):
        self.Name = name