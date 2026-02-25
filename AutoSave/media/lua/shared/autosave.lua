local SAVE_INTERVAL = 600
local lastSave = 0


-- =========================
-- LOG TXT (API NATIVA)
-- =========================
local function writeLog()

    local path = getFileWriter("autosave_log.txt", true, false)

    if path then
        local time = os.date("%d/%m/%Y %H:%M:%S")
        path:write("Save realizado em: "..time.."\n")
        path:close()
    end

end



-- =========================
-- SAVE
-- =========================
local function doSave()

    print("[AutoSave] Salvando jogo...")

    GameWindow.save(true)

    writeLog()

    print("[AutoSave] Save concluído!")

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