local function addLoot(distributions, container, item, chance)

    if not distributions[container] then return end
    if not distributions[container].items then return end

    table.insert(distributions[container].items, item)
    table.insert(distributions[container].items, chance)

end



local function insertLoot()

    local distributions = SuburbsDistributions

    if not distributions then
        print("[PacocaMod] ERRO: SuburbsDistributions nil")
        return
    end

    print("[PacocaMod] Inserindo Paçoca nas tabelas de loot")

    -- CASAS
    addLoot(distributions, "KitchenCounter", "Base.Pacoca", 3)
    addLoot(distributions, "KitchenCabinet", "Base.Pacoca", 3)

    -- LOJAS
    addLoot(distributions, "StoreShelfSnacks", "Base.Pacoca", 6)
    addLoot(distributions, "StoreShelfFood", "Base.Pacoca", 5)

    -- ESCOLAS
    addLoot(distributions, "SchoolLockers", "Base.Pacoca", 2)
    addLoot(distributions, "ClassroomDesk", "Base.Pacoca", 2)

    -- GERAL
    addLoot(distributions, "CrateRandomJunk", "Base.Pacoca", 1)

end


Events.OnPreDistributionMerge.Add(insertLoot)