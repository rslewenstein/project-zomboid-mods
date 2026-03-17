-- Função auxiliar para inserir loot com segurança na B42
local function addLoot(container, item, chance)
    local dist = ProceduralDistributions.list[container]
    if dist and dist.items then
        table.insert(dist.items, item)
        table.insert(dist.items, chance)
    else
        -- Log de depuração (útil na VM Linux para ver erros no terminal)
        print("[PacocaMod] Aviso: Container " .. tostring(container) .. " nao encontrado.")
    end
end

local function insertLoot()
    -- Na B42, focamos em ProceduralDistributions para itens de prateleira e armários
    print("[PacocaMod] Inserindo Paçoca no sistema procedural da B42")

    -- COZINHAS (Casas)
    addLoot("KitchenCounters", "Base.Pacoca", 4)
    addLoot("KitchenCabinets", "Base.Pacoca", 4)

    -- LOJAS E CONVENIÊNCIAS (Nomes de tabelas atualizados)
    addLoot("StoreShelfSnacks", "Base.Pacoca", 8)
    addLoot("GumpysItems", "Base.Pacoca", 6) -- Doces em postos de gasolina
    addLoot("VendingMachineSnacks", "Base.Pacoca", 10)

    -- ESCOLAS E LOCAIS PÚBLICOS
    addLoot("SchoolLockers", "Base.Pacoca", 3)
    addLoot("LockerPublic", "Base.Pacoca", 3)

    -- GERAL / ESTOQUE
    addLoot("CrateRandomJunk", "Base.Pacoca", 2)
end

-- O evento correto para a B42 é o OnInitGlobalModData ou OnPostDistributionMerge
Events.OnPostDistributionMerge.Add(insertLoot)
