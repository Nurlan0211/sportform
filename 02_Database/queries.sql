-- 1. Все контрагенты с типом
SELECT c.id, c.name, c.inn, c.phone, ct.name AS type_name
FROM counterparty c
JOIN counterparty_type ct ON c.type_id = ct.id
ORDER BY ct.name, c.name;

-- 2. Покупатели, у которых есть заказы
SELECT DISTINCT c.id, c.name, c.inn, c.phone
FROM counterparty c
JOIN customer_order o ON o.customer_id = c.id
WHERE c.type_id = 1
ORDER BY c.name;

-- 3. Поставщики и их материалы
SELECT c.name AS supplier, m.name AS material, m.unit, m.purchase_price
FROM material m
JOIN counterparty c ON m.supplier_id = c.id
ORDER BY c.name, m.name;

-- 4. Заказы: номер, дата, покупатель, продукция, кол-во, статус
SELECT o.order_number, o.order_date, c.name AS customer,
       p.name AS product, oi.quantity, o.status
FROM customer_order o
JOIN counterparty c ON o.customer_id = c.id
JOIN order_item oi ON oi.order_id = o.id
JOIN product p ON oi.product_id = p.id
ORDER BY o.order_date DESC;

-- 5. Стоимость каждого заказа
SELECT o.order_number, o.order_date, c.name AS customer,
       SUM(p.price * oi.quantity) AS order_sum
FROM customer_order o
JOIN counterparty c ON o.customer_id = c.id
JOIN order_item oi ON oi.order_id = o.id
JOIN product p ON oi.product_id = p.id
GROUP BY o.id, o.order_number, o.order_date, c.name
ORDER BY o.order_date DESC;

-- 6. Состав продукции (пример — id = 1)
SELECT p.name AS product, m.name AS material, s.quantity AS norm,
       m.unit, m.purchase_price,
       (s.quantity * m.purchase_price) AS material_cost_per_unit
FROM specification s
JOIN product p ON s.product_id = p.id
JOIN material m ON s.material_id = m.id
WHERE p.id = 1;

-- 7. Материальная себестоимость единицы продукции
SELECT p.id, p.name,
       COALESCE(SUM(s.quantity * m.purchase_price), 0) AS material_cost
FROM product p
LEFT JOIN specification s ON s.product_id = p.id
LEFT JOIN material m ON s.material_id = m.id
GROUP BY p.id, p.name
ORDER BY p.name;

-- 8. Стоимость операций по продукции
SELECT p.id, p.name,
       COALESCE(SUM(op.cost), 0) AS operations_cost
FROM product p
LEFT JOIN product_operation po ON po.product_id = p.id
LEFT JOIN operation op ON po.operation_id = op.id
GROUP BY p.id, p.name
ORDER BY p.name;

-- 9. Полная себестоимость (материалы + операции)
SELECT p.id, p.name,
       COALESCE(mat.material_cost, 0) AS material_cost,
       COALESCE(ops.operations_cost, 0) AS operations_cost,
       COALESCE(mat.material_cost, 0) + COALESCE(ops.operations_cost, 0) AS full_cost
FROM product p
LEFT JOIN (
    SELECT s.product_id, SUM(s.quantity * m.purchase_price) AS material_cost
    FROM specification s JOIN material m ON s.material_id = m.id
    GROUP BY s.product_id
) mat ON mat.product_id = p.id
LEFT JOIN (
    SELECT po.product_id, SUM(op.cost) AS operations_cost
    FROM product_operation po JOIN operation op ON po.operation_id = op.id
    GROUP BY po.product_id
) ops ON ops.product_id = p.id
ORDER BY p.name;

-- 10. Заказы в производстве / запланированные
SELECT o.order_number, o.order_date, c.name AS customer, o.status,
       po.production_number, po.status AS production_status
FROM customer_order o
JOIN counterparty c ON o.customer_id = c.id
LEFT JOIN production_order po ON po.customer_order_id = o.id
WHERE o.status IN ('В производстве', 'Подтверждён')
   OR po.status IN ('Запланирован', 'В работе')
ORDER BY o.order_date;

-- 11. Количество заказов по покупателям
SELECT c.name AS customer, COUNT(o.id) AS orders_count
FROM counterparty c
LEFT JOIN customer_order o ON o.customer_id = c.id
WHERE c.type_id = 1
GROUP BY c.id, c.name
ORDER BY orders_count DESC;

-- 12. Топ-3 продукции по сумме продаж
SELECT p.name AS product,
       SUM(oi.quantity) AS total_qty,
       SUM(p.price * oi.quantity) AS total_sales
FROM order_item oi
JOIN product p ON oi.product_id = p.id
GROUP BY p.id, p.name
ORDER BY total_sales DESC
LIMIT 3;