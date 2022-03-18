package com.example.xamarinbackend.controller;

import com.example.xamarinbackend.entities.Account;
import com.example.xamarinbackend.entities.IdEntity;
import com.example.xamarinbackend.entities.Note;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.PersistenceContext;
import javax.persistence.PersistenceUnit;
import java.util.UUID;

@Component
public class ServiceImpl implements Service {

    @PersistenceContext
    EntityManager entityManager;

    @Override
    @Transactional
    public void save(Account account) {
        entityManager.merge(account);
    }

    @Override
    @Transactional
    public Account get(UUID accountID) {
        return entityManager.find(Account.class, accountID);
    }

}
