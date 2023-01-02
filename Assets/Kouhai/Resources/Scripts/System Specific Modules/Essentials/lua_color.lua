--- This file holds Color table

---@class Color
Color = {}
Color.r = 0
Color.g = 0
Color.b = 0

--- Creates a new Color 
--- table format {r,g,b}
---@param colorTable table
---@return Color
function Color.New(colorTable)
    ---@type Color
    return {r=colorTable[1],g=colorTable[2],b=colorTable[3]}
end
