local SAVE_INTERVAL = 600
local lastSave = 0

-- =========================
-- LOG TXT (API NATIVA)
-- =========================
local function writeLog()
    local time = os.date("%d/%m/%Y %H:%M:%S")
    print("[AUTOSAVE] - Save realizado em: "..time.."\n")
end

-- =========================
-- SAVE
-- =========================
local function doSave()
    GameWindow.save(true)
    writeLog()
end

-- =========================
-- TIMER
-- =========================
Events.OnTick.Add(function()

    local now = os.time()

    if now - lastSave >= SAVE_INTERVAL then
        lastSave = now
        doSave()
    end

end)