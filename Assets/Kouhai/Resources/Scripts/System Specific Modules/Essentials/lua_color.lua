--- This file holds Color table

Color = {r=0,g=0,b=0}

--- Creates a new Color 
--- table format {r,g,b}
--- @param r number
--- @param g number
--- @param b number
--- @return Color
function Color:New(r,g,b)
    o = {}
    setmetatable(o, self)
    self.__index = self
    self.r = r or 0
    self.g = g or 0
    self.b = b or 0
    return o
end

--- Red Color table
function Color:Red()
   return Color.New(1,0,0)
end

--- Green color table
function Color:Green()
    return Color.New(0,1,0)
end

--- Blue color table
function Color:Blue()
    return Color.New(0,0,1)
end

--- White color table
function Color:White()
    return Color.New(1,1,1)
end

--- Black color table
function Color:Black()
    return Color.New(0,0,0)
end
