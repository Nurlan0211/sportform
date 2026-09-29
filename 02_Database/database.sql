SELECT c.id, c.name, c.inn, c.address, c.phone, ct.name AS тип
FROM counterparty c
JOIN counterparty_type ct ON c.type_id = ct.id
ORDER BY ct.name, c.name;