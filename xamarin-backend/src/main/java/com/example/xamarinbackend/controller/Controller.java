package com.example.xamarinbackend.controller;

import com.example.xamarinbackend.entities.Account;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.UUID;

@RestController
@CrossOrigin(origins = "*")
public class Controller {

    @Autowired
    Service service;

    @PostMapping("api/push")
    public void push(@RequestBody Account account) {
        service.save(account);
    }

    @GetMapping("api/pull")
    public Account pull(@RequestParam UUID accountID) {
        return service.get(accountID);
    }

}
