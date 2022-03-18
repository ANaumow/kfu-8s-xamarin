package com.example.xamarinbackend.controller;

import com.example.xamarinbackend.entities.Account;

import java.util.UUID;

public interface Service {
    void save(Account account);

    Account get(UUID accountID);
}
