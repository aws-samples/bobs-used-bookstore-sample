-- ------------ Write CREATE-DATABASE-stage scripts -----------

-- Install CITEXT extension for case-insensitive text support
CREATE EXTENSION IF NOT EXISTS citext;

CREATE SCHEMA IF NOT EXISTS bobsusedbookstore_dbo;

-- ------------ Write CREATE-TABLE-stage scripts -----------

CREATE TABLE bobsusedbookstore_dbo.address(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    addressline1 CITEXT NOT NULL,
    addressline2 CITEXT,
    city CITEXT NOT NULL,
    state CITEXT NOT NULL,
    country CITEXT NOT NULL,
    zipcode CITEXT NOT NULL,
    customerid INTEGER NOT NULL,
    isactive INTEGER NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.book(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    name CITEXT NOT NULL,
    author CITEXT NOT NULL,
    year INTEGER,
    isbn CITEXT NOT NULL,
    publisherid INTEGER NOT NULL,
    booktypeid INTEGER NOT NULL,
    genreid INTEGER NOT NULL,
    conditionid INTEGER NOT NULL,
    coverimageurl CITEXT,
    summary CITEXT,
    price NUMERIC(18,2) NOT NULL,
    quantity INTEGER NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.customer(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    sub CITEXT NOT NULL,
    username CITEXT,
    firstname CITEXT,
    lastname CITEXT,
    email CITEXT,
    dateofbirth TIMESTAMP(6) WITHOUT TIME ZONE,
    phone CITEXT,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.offer(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    author CITEXT NOT NULL,
    isbn CITEXT NOT NULL,
    bookname CITEXT NOT NULL,
    fronturl CITEXT,
    genreid INTEGER NOT NULL,
    conditionid INTEGER NOT NULL,
    publisherid INTEGER NOT NULL,
    booktypeid INTEGER NOT NULL,
    summary CITEXT,
    offerstatus INTEGER NOT NULL,
    comment CITEXT,
    customerid INTEGER NOT NULL,
    bookprice NUMERIC(18,2) NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.orderitem(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    orderid INTEGER NOT NULL,
    bookid INTEGER NOT NULL,
    quantity INTEGER NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.orders(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    customerid INTEGER NOT NULL,
    addressid INTEGER NOT NULL,
    deliverydate TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    orderstatus INTEGER NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.referencedata(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    datatype INTEGER NOT NULL,
    text CITEXT NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.shoppingcart(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    correlationid CITEXT NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

CREATE TABLE bobsusedbookstore_dbo.shoppingcartitem(
    id INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY,
    shoppingcartid INTEGER NOT NULL,
    bookid INTEGER NOT NULL,
    quantity INTEGER NOT NULL,
    wanttobuy INTEGER NOT NULL,
    createdby CITEXT NOT NULL,
    createdon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    updatedon TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL
)
        WITH (
        OIDS=FALSE
        );

-- ------------ Write CREATE-INDEX-stage scripts -----------

CREATE INDEX ix_address_ix_address_customerid
ON bobsusedbookstore_dbo.address
USING BTREE (customerid ASC);

CREATE INDEX ix_book_ix_book_booktypeid
ON bobsusedbookstore_dbo.book
USING BTREE (booktypeid ASC);

CREATE INDEX ix_book_ix_book_conditionid
ON bobsusedbookstore_dbo.book
USING BTREE (conditionid ASC);

CREATE INDEX ix_book_ix_book_genreid
ON bobsusedbookstore_dbo.book
USING BTREE (genreid ASC);

CREATE INDEX ix_book_ix_book_publisherid
ON bobsusedbookstore_dbo.book
USING BTREE (publisherid ASC);

CREATE UNIQUE INDEX ix_customer_ix_customer_sub
ON bobsusedbookstore_dbo.customer
USING BTREE (sub ASC);

CREATE INDEX ix_offer_ix_offer_booktypeid
ON bobsusedbookstore_dbo.offer
USING BTREE (booktypeid ASC);

CREATE INDEX ix_offer_ix_offer_conditionid
ON bobsusedbookstore_dbo.offer
USING BTREE (conditionid ASC);

CREATE INDEX ix_offer_ix_offer_customerid
ON bobsusedbookstore_dbo.offer
USING BTREE (customerid ASC);

CREATE INDEX ix_offer_ix_offer_genreid
ON bobsusedbookstore_dbo.offer
USING BTREE (genreid ASC);

CREATE INDEX ix_offer_ix_offer_publisherid
ON bobsusedbookstore_dbo.offer
USING BTREE (publisherid ASC);

CREATE INDEX ix_orderitem_ix_orderitem_bookid
ON bobsusedbookstore_dbo.orderitem
USING BTREE (bookid ASC);

CREATE INDEX ix_orderitem_ix_orderitem_orderid
ON bobsusedbookstore_dbo.orderitem
USING BTREE (orderid ASC);

CREATE INDEX ix_orders_ix_orders_addressid
ON bobsusedbookstore_dbo.orders
USING BTREE (addressid ASC);

CREATE INDEX ix_orders_ix_orders_customerid
ON bobsusedbookstore_dbo.orders
USING BTREE (customerid ASC);

CREATE INDEX ix_shoppingcartitem_ix_shoppingcartitem_bookid
ON bobsusedbookstore_dbo.shoppingcartitem
USING BTREE (bookid ASC);

CREATE INDEX ix_shoppingcartitem_ix_shoppingcartitem_shoppingcartid
ON bobsusedbookstore_dbo.shoppingcartitem
USING BTREE (shoppingcartid ASC);

-- ------------ Write CREATE-CONSTRAINT-stage scripts -----------

ALTER TABLE bobsusedbookstore_dbo.address
ADD CONSTRAINT pk_address_901578250 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.book
ADD CONSTRAINT pk_book_933578364 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.customer
ADD CONSTRAINT ck_customer_len_sub CHECK (length(sub::text) <= 450);

ALTER TABLE bobsusedbookstore_dbo.customer
ADD CONSTRAINT pk_customer_965578478 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT pk_offer_997578592 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.orderitem
ADD CONSTRAINT pk_orderitem_1029578706 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.orders
ADD CONSTRAINT pk_orders_1061578820 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.referencedata
ADD CONSTRAINT pk_referencedata_1093578934 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.shoppingcart
ADD CONSTRAINT pk_shoppingcart_1125579048 PRIMARY KEY (id);

ALTER TABLE bobsusedbookstore_dbo.shoppingcartitem
ADD CONSTRAINT pk_shoppingcartitem_1157579162 PRIMARY KEY (id);

-- ------------ Write CREATE-FOREIGN-KEY-CONSTRAINT-stage scripts -----------

ALTER TABLE bobsusedbookstore_dbo.address
ADD CONSTRAINT fk_address_customer_customerid_1173579219 FOREIGN KEY (customerid) 
REFERENCES bobsusedbookstore_dbo.customer (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.book
ADD CONSTRAINT fk_book_referencedata_booktypeid_1189579276 FOREIGN KEY (booktypeid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.book
ADD CONSTRAINT fk_book_referencedata_conditionid_1205579333 FOREIGN KEY (conditionid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.book
ADD CONSTRAINT fk_book_referencedata_genreid_1221579390 FOREIGN KEY (genreid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.book
ADD CONSTRAINT fk_book_referencedata_publisherid_1237579447 FOREIGN KEY (publisherid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT fk_offer_customer_customerid_1253579504 FOREIGN KEY (customerid) 
REFERENCES bobsusedbookstore_dbo.customer (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT fk_offer_referencedata_booktypeid_1269579561 FOREIGN KEY (booktypeid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT fk_offer_referencedata_conditionid_1285579618 FOREIGN KEY (conditionid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT fk_offer_referencedata_genreid_1301579675 FOREIGN KEY (genreid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.offer
ADD CONSTRAINT fk_offer_referencedata_publisherid_1317579732 FOREIGN KEY (publisherid) 
REFERENCES bobsusedbookstore_dbo.referencedata (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.orderitem
ADD CONSTRAINT fk_orderitem_book_bookid_1333579789 FOREIGN KEY (bookid) 
REFERENCES bobsusedbookstore_dbo.book (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.orderitem
ADD CONSTRAINT fk_orderitem_orders_orderid_1349579846 FOREIGN KEY (orderid) 
REFERENCES bobsusedbookstore_dbo.orders (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.orders
ADD CONSTRAINT fk_orders_address_addressid_1365579903 FOREIGN KEY (addressid) 
REFERENCES bobsusedbookstore_dbo.address (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.orders
ADD CONSTRAINT fk_orders_customer_customerid_1381579960 FOREIGN KEY (customerid) 
REFERENCES bobsusedbookstore_dbo.customer (id)
ON UPDATE NO ACTION
ON DELETE NO ACTION;

ALTER TABLE bobsusedbookstore_dbo.shoppingcartitem
ADD CONSTRAINT fk_shoppingcartitem_book_bookid_1397580017 FOREIGN KEY (bookid) 
REFERENCES bobsusedbookstore_dbo.book (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

ALTER TABLE bobsusedbookstore_dbo.shoppingcartitem
ADD CONSTRAINT fk_shoppingcartitem_shoppingcart_shoppingcartid_1413580074 FOREIGN KEY (shoppingcartid) 
REFERENCES bobsusedbookstore_dbo.shoppingcart (id)
ON UPDATE NO ACTION
ON DELETE CASCADE;

