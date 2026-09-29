SELECT
    o.order_number AS "Номер заказа",
    o.order_date AS "Дата",
    c.name AS "Заказчик",

    ROUND(
        SUM(
            si.quantity
            * pr.price
            * oi.quantity
        ),
        2
    ) AS "Полная стоимость заказа"

FROM orders o

JOIN counterparties c
    ON c.counterparty_id = o.customer_id

JOIN order_items oi
    ON oi.order_id = o.order_id

JOIN products p
    ON p.product_id = oi.product_id

JOIN specifications s
    ON s.product_id = p.product_id

JOIN specification_items si
    ON si.specification_id = s.specification_id

JOIN products m
    ON m.product_id = si.material_id

JOIN prices pr
    ON pr.product_id = m.product_id

GROUP BY
    o.order_id,
    o.order_number,
    o.order_date,
    c.name

ORDER BY
    o.order_date;
